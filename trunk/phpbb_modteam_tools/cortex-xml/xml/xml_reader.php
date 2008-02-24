<?php
/**
 * XML reader
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 *
 * @copyright	2006 Coronis - http://www.coronis.nl
 * @license		http://www.gnu.org/licenses/lgpl.html GNU Lesser General Public License
 * 				See copyright.txt for more information
 *
 * @version		$Id: xml_reader.php,v 1.1 2008-02-24 20:16:04 smithydll Exp $
 */

/**
 * cortex_xml_reader class
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 */
class cortex_xml_reader extends cortex_xml
{
	/**
	 * Raw XML data
	 *
	 * @access	public
	 * @var		string
	 */
	var $xml_raw;

	/**
	 * Root XML element (object of cortex_xml_element)
	 *
	 * @access	public
	 * @var		object
	 */
	var $root_element;

	/**
	 * Parse an XML file
	 *
	 * @access	public
	 * @param	string		Filename
	 * @return	object		Root XML element (object of cortex_xml_element)
	 */
	function parse_file($filename)
	{
		$this->xml_raw = @file_get_contents($filename);
		if ( $this->xml_raw === false )
		{
			new cortex_exception('cortex_xml_reader: Could not parse XML', 'Input file "' . $filename . '" could not be loaded');
		}

		return $this->parse_data();
	}

	/**
	 * Parse the XML data in $this->xml_raw
	 *
	 * @access	public
	 * @return	object		Root XML element (object of cortex_xml_element)
	 */
	function parse_data($xml_raw = '')
	{
		if ( !empty($xml_raw) )
		{
			$this->xml_raw = $xml_raw;
		}

		$parser = xml_parser_create();

		xml_parser_set_option($parser, XML_OPTION_CASE_FOLDING, 0);
		xml_parser_set_option($parser, XML_OPTION_SKIP_WHITE, 0);
		xml_parse_into_struct($parser, $this->xml_raw, $tags);

		xml_parser_free($parser);

		$elements = array();
		$stack = array();

		foreach ( $tags as $tag )
		{
			$index = sizeof($elements);

			if ( $tag['type'] == 'complete' || $tag['type'] == 'open' )
			{
				$elements[$index] = new cortex_xml_element();

				$elements[$index]->name = $tag['tag'];
				if (array_key_exists('attributes', $tag))
					$elements[$index]->attributes = $tag['attributes'];
				if (array_key_exists('value', $tag))
					$elements[$index]->content = trim($tag['value']);

				if ( $tag['type'] == 'open' )
				{
					$elements[$index]->children = array();
					$stack[sizeof($stack)] = &$elements;
					$elements = &$elements[$index]->children;
				}
			}

			if ( $tag['type'] == 'close' )
			{
				$elements = &$stack[sizeof($stack) - 1];
				unset($stack[sizeof($stack) - 1]);
			}
		}

		$this->root_element = $elements[0];

		return $this->root_element;
	}
}
?>
