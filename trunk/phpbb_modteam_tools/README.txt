THIS SOFTWARE IS COPYRIGHT DAVID LACHLAN SMITH http://smithydll.id.au/
LICENSED UNDER THE GNU GPL v3

This is a port of the c# library ModTemplateTools, the new namespace starting in the latter half of 2006
will be Phpbb.ModTeam.Tools.

Make sure to use camel case & pascal case, and keep the original algorithms where possible. If you change
an algorithm, test is thrououghly and document it. The algorithms provided are just as battle tested as
easyMOD.

The namespace for this will be changed in the future. But for the moment just use as is now.
The procedures are what is important, not the namespace.

DOCUMENT EVERYTHING YOU WRITE USING XML COMMENTS =>
it is sufficient to just copy and paste the ones from the c# source.

Please make sure to document, I want the c# source and the php source to keep in sync. You can grab a
copy of the c# source on the modstudio project CVS on sourceforce.

That is all for the moment.

(The documentation is important because it gets compiled into a chm help document, it would be useful if
the one help document applied to both versions of the source)

/*******************************************
 * INSTRUCTIONS FOR MODIFYING THIS LIBRARY *
 *******************************************
 *
 * This code lives in the mod studio repository, make sure that this
 * repository contains the most up-to-date version of this code. Do not work
 * out of any other repositories.
 *
 * Bug fixes in the use of the PHP language
 *  - You may make these bug fixes at any time so long as you comitt the
 *    fixes to the correct repository.
 *
 * Refactoring code to suit project coding guidelines
 *  - Don't ask, just do it (please).
 *  - Inform everyone that you've done it so they can adjust any hooks their
 *    programme has into this library.
 *  - Remove this part of the message once the library has been refactored
 *  - See Also: http://area51.phpbb.com/docs/coding-guidelines.html
 *
 * Making changes to the algorithm
 *  - Changes in the algorithm must not be made without applying applicable
 *    changes to the c# version of this code first. The two code bases must
 *    remain in sync with each other. This ensures that all tools behave
 *    in the same predictable manner. If you cannot do this, ask someone who
 *    can.
 *
 * Accept Copyright Assignment
 * - All changes need to be signed over to copyright under my name before they
 *	 will be included in the official trunk.
 * - Authors retain all moral rights where applicable by law.
 *
 */

-------------------------------------------------------------------------------
UPDATE (8:10 PM 15/04/2006): Phpbbmod.php is now defunct

I have completed updating the namespace for the c# version and have started work on translating from that.