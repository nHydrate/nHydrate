$(document).ready(function () {

	if ($.browser.msie)
		$('.fadein').show(); //Fade In does not work in IE
	else
		$('.fadein').hide().fadeIn();

	//Determine if the browser is IE and less than version 9
	var bversion = parseInt($.browser.version, 10);
	var msie8OrUnder = ($.browser.msie != null) && (bversion < 9);
	if (!msie8OrUnder) {
		$('.roundblock').corner('10px');
		$('.roundblock > .h').corner('10px top');

		//Small round corners
		$('.roundblocksmall').corner('5px');
		$('.roundblocksmall > .h').corner('5px top');

		$('.roundblocksmalltop').corner('5px top');
		$('.roundblocksmallbottom').corner('5px bottom');
	} //IE

});
