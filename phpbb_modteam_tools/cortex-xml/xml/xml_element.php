<?php
/**
 * XML element
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 *
 * @copyright	2006 Coronis - http://www.coronis.nl
 * @license		http://www.gnu.org/licenses/lgpl.html GNU Lesser General Public License
 * 				See copyright.txt for more information
 *
 * @version		$Id: xml_element.php,v 1.1 2008-02-24 20:16:04 smithydll Exp $
 */

/**
 * cortex_xml_element class
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 */
class cortex_xml_element
{
	/**
	 * Name of the XML element
	 *
	 * @access	public
	 * @var		string
	 */
	var $name;

	/**
	 * Element attributes
	 *
	 * @access	public
	 * @var		array
	 */
	var $attributes;

	/**
	 * Element's contents
	 *
	 * @access	public
	 * @var		string
	 */
	var $content;

	/**
	 * Element's child elements
	 *
	 * @access	public
	 * @var		array
	 */
	var $children;
	
	var $root = false;

	/**
	 * cortex_xml_element constructor
	 *
	 * @access	public
	 * @param	string		Element name
	 * @return	void
	 */
	function cortex_xml_element($name = '')
	{
		$this->name = $name;
	}

	/**
	 * Find a child element by its name
	 *
	 * @access	public
	 * @param	string		Child name
	 * @return	mixed		Child object on success, otherwise FALSE
	 */
	function get_child($child_name, $get_contents = false)
	{
		for ( $i = 0, $_i = sizeof($this->children); $i < $_i; $i++ )
		{
			if ( $this->children[$i]->name == $child_name )
			{
				if ( $get_contents )
				{
					return $this->children[$i]->content;
				}
				else
				{
					return $this->children[$i];
				}
			}
		}

		return false;
	}

	function get_xml($include_doctype = true)
	{
		$xml_writer = cortex_xml::factory('writer');
		return $xml_writer->format_data($this, $include_doctype);
	}

	function output_xml($no_cache = false, $exit = false)
	{
		$xml_writer = cortex_xml::factory('writer');
		$xml_writer->format_data($this, true);
		$xml_writer->output_data($no_cache, $exit);
	}

	function add_attribute($attribute_name, $attribute_value)
	{
		$this->attributes[$attribute_name] = $attribute_value;
	}

	function set_attributes($attributes)
	{
		$this->attributes = $attributes;
	}

	function add_child(&$child)
	{
		@$this->children[] =& $child;
	}

	function set_parent($parent)
	{
		$parent->children[] = $this;
	}

	function get_child_by_attribute($attribute_name, $attribute_value, $get_contents = false)
	{
		for ( $i = 0, $_i = sizeof($this->children); $i < $_i; $i++ )
		{
			if ( isset($this->children[$i]->attributes[$attribute_name]) && $this->children[$i]->attributes[$attribute_name] == $attribute_value )
			{
				if ( $get_contents )
				{
					return $this->children[$i]->content;
				}
				else
				{
					return $this->children[$i];
				}
			}
		}

		return false;
	}
}
?>
