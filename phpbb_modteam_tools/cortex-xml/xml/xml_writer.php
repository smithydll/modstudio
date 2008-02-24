<?php
/**
 * XML writer
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 *
 * @copyright	2006 Coronis - http://www.coronis.nl
 * @license		http://www.gnu.org/licenses/lgpl.html GNU Lesser General Public License
 * 				See copyright.txt for more information
 *
 * @version		$Id: xml_writer.php,v 1.1 2008-02-24 20:16:04 smithydll Exp $
 */

/**
 * cortex_xml_writer class
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 */
class cortex_xml_writer extends cortex_xml
{
	/**
	 * Raw XML data
	 *
	 * @access	protected
	 * @var		string
	 */
	var $xml_data = '';

	var $xml_indent = "  ";

	var $xml_stylesheet = '';

	var $xml_charset = 'utf-8';

	/**
	 * Convert a cortex_xml_element into raw XML
	 *
	 * @access	public
	 * @param	object		cortex_xml_element object
	 * @param	int			Number of tabs we should indent
	 * @return	string		XML
	 */
	function object_to_xml($object, $indent = 0)
	{
		$data = '';
		if ($object->root === true)
		{
			$data .= '<?xml version="1.0" encoding="' . $this->xml_charset . '" standalone="yes"?>';
			$data .= "\n";
			if ($this->xml_stylesheet != '')
			{
				$data .= '<?xml-stylesheet type="text/xsl" href="' . $this->xml_stylesheet . '"?>';
			}
			if ($object->name == 'mod') // root element
			{
				$data .= "\n";
				$data .= '<!--For security purposes, please check: http://www.phpbb.com/mods/ for the latest version of this MOD. Although MODs are checked before being allowed in the MODs Database there is no guarantee that there are no security problems within the MOD. No support will be given for MODs not found within the MODs Database which can be found at http://www.phpbb.com/mods/-->';
			}
			//$data .= '<mod xmlns="http://www.phpbb.com/mods/xml/modx-1.0.xsd"';
			$object->add_attribute('xmlns:xsi', 'http://www.w3.org/2001/XMLSchema-instance');
			$data .= "\n" . str_pad('', $indent, $this->xml_indent, STR_PAD_LEFT) . '<' . $object->name;
		}
		else
		{
			$data .= "\n" . str_pad('', $indent, $this->xml_indent, STR_PAD_LEFT) . '<' . $object->name;
		}
		$indent += strlen($this->xml_indent);

		// Attributes
		if ( is_array($object->attributes) )
		{
			foreach ( $object->attributes as $attribute => $attribute_value )
			{
				$data .= ' ' . $attribute . '="' . $attribute_value . '"';
			}
		}

		if (is_array($object->children) || is_array($object->content) || !($object->content === null))
		{

			$data .= ">";

			// Children
			if ( is_array($object->children) )
			{
				foreach ( $object->children as $child_element )
				{
					$data .= $this->object_to_xml($child_element, $indent);
				}
			}

			// Content (array)... this is a quick 'n dirty way of having multiple
			// elements
			elseif ( is_array($object->content) )
			{
				foreach ( $object->content as $element => $element_value )
				{
					$data .= str_pad('', $indent, $this->xml_indent, STR_PAD_LEFT) . '<' . $element . ">" . $this->cdata($element_value) . '</' . $element . ">\n";
				}
			}

			// Content (string)
			else
			{
				$data .= $this->cdata($object->content);
			}

			// Closing tag
			$indent -= strlen($this->xml_indent);
			if ( is_array($object->children) )
			{
			$data .= "\n" . str_pad('', $indent, $this->xml_indent, STR_PAD_LEFT) . '</' . $object->name . '>';
			}
			else
			{
			$data .= '</' . $object->name . '>';
			}
		}
		else
		{
			$data .= " />";
		}

		return $data;
	}

	/**
	 * Return <![CDATA[$contents]]>
	 *
	 * @access	protected
	 * @param	string		Contents to escape
	 * @return	string		Escaped contents on success, otherwise original string
	 */
	function cdata($contents)
	{
		$contents = ( $this->xml_encoding == 'utf-8' ) ? utf8_encode(trim($contents)) : trim($contents);

		if ( preg_match('/\<(.*?)\>/xsi', $contents) )
		{
			$contents = preg_replace('/\<script[\s]+(.*)\>(.*)\<\/script\>/xsi', '', $contents);
		}

		if (!(strpos($contents, '>') === false) || !(strpos($contents, '<') === false) || !(strpos($contents, '&') === false))
		{
			// CDATA doesn't let you use ']]>' so fall back to WriteString
			if (!(strpos($contents, ']]>') === false))
			{
				return htmlspecialchars($contents);
			}
			else
			{
				return '<![CDATA[' . $contents . ']]>';
			}
		}
		else
		{
			return htmlspecialchars($contents);
		}

	}

	/**
	 * Convert an object of cortex_xml_element and its children to a valid XML file
	 *
	 * @access	public
	 * @param	object		Object of cortex_xml_element
	 * @return	string		XML data
	 */
	function format_data($object, $include_doctype)
	{
		$this->xml_data = '';

		if ( $include_doctype )
		{
			$this->xml_data = '<?xml version="1.0" encoding="' . $this->xml_encoding . '"?>' . "\n";
		}

		$this->xml_data .= $this->object_to_xml($object);

		return $this->xml_data;
	}

	/**
	 * Directly output the data to the client
	 *
	 * @access	public
	 * @param	bool		Set to TRUE to send the 'no-cache' header. Should be done in order
	 * 						to prevent headaches and other mental disorders when using AJAX in IE
	 * @param	bool		Set to TRUE to call exit() after having sent the data
	 * @return	null
	 */
	function output_data($no_cache = false, $exit = false)
	{
		ob_clean();
		header('Content-type: text/xml');

		if ( $no_cache )
		{
			header('Pragma: no-cache');
			header('Cache-Control: no-cache');
			header('Expires: ' . date('r', 0));
		}

		print $this->xml_data;

		if ( $exit )
		{
			exit();
		}
	}

	/**
	 * Write $this->xml_data to a file
	 *
	 * @access	public
	 * @param	string		filename to write to
	 * @return	void
	 */
	function save_file($filename)
	{
		if ( !@file_put_contents($filename, $this->xml_data) )
		{
			new cortex_exception('Could not store XML data', 'Could not write to "' . $filename . '"');
		}
	}
}
?>
