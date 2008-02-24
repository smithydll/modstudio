<?php
/**
 * XML class
 *
 * @package		Cortex
 * @subpackage	cortex_xml
 *
 * @copyright	2006 Coronis - http://www.coronis.nl
 * @license		http://www.gnu.org/licenses/lgpl.html GNU Lesser General Public License
 * 				See copyright.txt for more information
 *
 * @version		$Id: xml.php,v 1.1 2008-02-24 20:16:04 smithydll Exp $
 */

/**
 * We also need the cortex_xml_element class
 */
//require(CORTEX_PATH . 'cortex/xml/xml_element.php');

/**
 * cortex_xml class
 *
 * @abstract
 * @package		Cortex
 * @subpackage	cortex_xml
 */
class cortex_xml
{
	/**
	 * XML encoding
	 *
	 * @access	protected
	 * @var		string
	 */
	var $xml_encoding = '';

	/**
	 * Setup $type XML class
	 *
	 * @final
	 * @access	public
	 * @param	string		XML class to load
	 * @param	string		XML encoding
	 * @return	object		New xml object
	 */
	function factory($type, $xml_encoding = 'utf-8')
	{
		$xml_class = 'cortex_xml_' . $type;

		if ( !cortex_util::class_exists($xml_class) )
		{
			if ( !file_exists(CORTEX_PATH . 'cortex/xml/xml_' . $type . '.php') )
			{
				new cortex_exception('cortex_xml: Could not initialize XML class', 'File for XML operation "' . $type . '" could not be found');
			}

			require(CORTEX_PATH . 'cortex/xml/xml_' . $type . '.php');
		}

		$xml_object = new $xml_class();
		$xml_object->set_encoding($xml_encoding);

		return $xml_object;
	}

	/**
	 * Set content encoding
	 *
	 * @access	protected
	 * @param	string		XML encoding
	 * @return	void
	 */
	function set_encoding($xml_encoding)
	{
		$this->xml_encoding = trim($xml_encoding);
	}
}
?>