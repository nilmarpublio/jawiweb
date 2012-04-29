/*
 * TODO: jquery write to file see http://jquery.tiddlywiki.org/twFile.html
 */

/*
jQuery XML Parser with Sorting and Filtering
Written by Ben Lister (darkcrimson.com) revised 25 Apr 2010
Tutorial: http://blog.darkcrimson.com/2010/01/jquery-xml-parser/

Licensed under the MIT License:
http://www.opensource.org/licenses/mit-license.php
*/

/**
 * Set the name become strikethrough when the checkbox is checked otherwise not.
 * 
 * @param chk
 *            String A checkbox id.
 */
function setStrikeThrough(chk) {
	// alert('setStrikeThrough');
	var id = '#';
	id += chk;
	// get correct id then addClass('strikethrough');
	var target = '#item_';
	var ids = id.split('_');
	if (ids.length > 1) {
		target += ids[1];
		if ($(id).is(':checked')) {
			$(target).addClass('strikethrough');
		} else {
			$(target).removeClass('strikethrough');
		}
	}
}

/**
 * Convert to roman letter. ie. ali bin ahmad -> Ali bin Ahmad.
 */
function toRoman(name) {
	var roman = '';
	var words = name.toLowerCase().split(' ');
	$.each(words, function(index, value) {

		// except bin or bt
		if (value == 'bin' || value == 'bt') {
			roman += value;
			roman += ' ';
		} else {
			var word = '';
			word += value.substring(0, 1).toUpperCase();
			word += value.substring(1, value.length);

			roman += word;
			roman += ' ';
		}
	});
	roman = roman.trim();

	return roman;
}

/**
 * True if it is a valid entry otherwise false.
 * 
 * @param name
 *            String Death person name.
 * @param item
 *            String Item type.
 */
function isValidate(name, item) {
	if (name.toLowerCase().indexOf('bin') >= 0 && item.toUpperCase().indexOf('(P)') >= 0)
		return false;
	if (name.toLowerCase().indexOf('bt') >= 0 && item.toUpperCase().indexOf('(L)') >= 0)
        return false;

	return true;
}

/**
 * Generate html value for table's row. This is for filtering use.
 * 
 * @param year
 *            Current year value.
 * @param index
 *            Numbering index.
 * @param node
 *            Xml node
 */
function generateRow(year,index,node) {

	var output = '';

	// property fields
	var xml_soldto = $(node).attr('soldto');
	var xml_deliver = $(node).attr('delivered');
	var xml_order = $(node).attr('date');
	var xml_price = $(node).attr('price');
	var xml_name = $(node).find('name').text();
	var xml_jawi = $(node).find('jawi').text();
	var xml_born = $(node).find('born').text();
	var xml_death = $(node).find('death').text();
	var xml_deathm = $(node).find('deathm').text();
	var xml_age = $(node).find('age').text();
	var xml_remarks = $(node).find('remarks').text();
	var xml_item = $(node).find('item').text();

	// compute description
	var desc = '';
	if(xml_remarks != '') {
		desc += "*";
	}
	desc += toRoman(xml_name);
	desc += ' (';
	if (xml_born != '') {
		desc += xml_born;
		desc += '~';
	}
	desc += xml_death;
	if (xml_deathm != '') {
		desc += '=';
		desc += xml_deathm;
	}
	desc += ') ';

	if (xml_age != '') {
		desc += ' ';
		desc += xml_age;
		desc += ' thn ';
	}
	desc += xml_jawi;

	// able to filter upon soldto person and month only
    // compute <tr filterCriteria='ADI07'
	output += '<tr filterCriteria="';
	if (xml_order.substring(0, 4) == year) {
	    output += xml_soldto;
	    output += xml_order.substring(5, 7); // extract only month value
	}
	output += '"';

	// add color style
	output += ' class="';
	if (!isValidate(xml_name, xml_item)) {
	    output += 'red';
	} else if (xml_item.indexOf("\u00BD") > -1) { //½
	    output += 'italic';
	} else if (xml_item.indexOf('Batik') > -1) {
	    output += 'blue';
	} else if (xml_item.indexOf('Hijau') > -1) {
	    output += 'green';
	} else if (xml_item.indexOf('Putih') > -1
			|| xml_item.indexOf('Marble') > -1) {
	    output += 'white';
	}
	output += '"';
	output += '>';
	
    // Currently not remember after sorting
	output += '<td>';
	output += index + '.';
	output += '</td>';

	output += '<td>';
	output += xml_soldto;
	output += '</td>';

	output += '<td>';
	output += xml_order;
	output += '</td>';

	output += '<td>';
	output += desc;
	output += '</td>';

	output += '<td>';
	output += xml_item;
	output += '</td>';

	output += '<td>';
	output += '<input type="checkbox"/>';
	output += '</td>';

	// get aging value
	var aging = '';
	if (xml_order.length == 10) {

		if (xml_deliver.length == 10) {
			// deliver case
			var year = xml_order.substring(0, 4);
			var month = xml_order.substring(5, 7);
			var day = xml_order.substring(8, 10);
			var from = new Date();
			from.setFullYear(year, month - 1, day);

			var year = xml_deliver.substring(0, 4);
			var month = xml_deliver.substring(5, 7);
			var day = xml_deliver.substring(8, 10);
			var to = new Date();
			to.setFullYear(year, month - 1, day);

			aging = getDifferentDays(from, to);
		} else {
			// compare to now
			var year = xml_order.substring(0, 4);
			var month = xml_order.substring(5, 7);
			var day = xml_order.substring(8, 10);
			var from = new Date();
			from.setFullYear(year, month - 1, day);

			aging = getDifferentDays(from, new Date());
		}
	}
	output += '<td>';
	output += aging;
	output += '</td>';

	output += '</tr>';

	return output;
}

/**
 * Return HTML string for table structure.
 * 
 * @param xml_list
 * @param soldto
 *            String soldto person.
 */
function parseXml(xml_list, soldto) {

	var xmlArr = [];
	var xml_no = 0; // numbering use

	var date = new Date();
	var year = date.getYear() - 100 + 2000;

	if (soldto == 'ALL' || soldto == '') {
	    $(xml_list).find('order').each(function () {
	        xml_no++;
	        xmlArr += generateRow(year,xml_no,this);
	    });  // end loops
	} else {
	$(xml_list).find('order').each(function () {
	    var xml_soldto = $(this).attr('soldto');
	    if (xml_soldto == soldto) {
	        xml_no++;
	        xmlArr += generateRow(year,xml_no,this);
	    }
	});  // end loop
	}

	return xmlArr;
}

/**
 * Compute html value with checkbox behind. This is for pending list use.
 *
 * @param year
 *            Current year value 
 * @param id
 *            Unique id
 * @param XmlNodenode
 *            Xml node
 */
function generateCheckBoxRow(year,id,node) {

	var output = '';
	// property fields
	var xml_soldto = $(node).attr('soldto');
	var xml_deliver = $(node).attr('delivered');
	var xml_order = $(node).attr('date');
	var xml_price = $(node).attr('price');
	var xml_name = $(node).find('name').text();
	var xml_jawi = $(node).find('jawi').text();
	var xml_born = $(node).find('born').text();
	var xml_death = $(node).find('death').text();
	var xml_deathm = $(node).find('deathm').text();
	var xml_age = $(node).find('age').text();
	var xml_remarks = $(node).find('remarks').text();
	var xml_item = $(node).find('item').text();

	// compute desciption
	var desc = '';
	if(xml_remarks != '') {
		desc += "*";
	}	
	desc += toRoman(xml_name);// romanize name
	desc += ' (';
	if (xml_born != '') {
		desc += xml_born;
		desc += '~';
	}
	desc += xml_death;
	if (xml_deathm != '') {
		desc += '=';
		desc += xml_deathm;
	}
	desc += ') ';

	if (xml_age != '') {
		desc += ' ';
		desc += xml_age;
		desc += ' thn ';
	}
	desc += xml_jawi;

	// begin compute each row of content
	output += '<tr';

	// add id for each row
	output += ' id="item_' + id + '"';

	// able to filter upon soldto person and month only
	output += ' filterCriteria="';
	if (xml_order.substring(0, 4) == year) {
	    output += xml_soldto;
	    output += xml_order.substring(5, 7); // extract only month value
	}
	output += '"';

	// add color style
	output += ' class="';
	if (!isValidate(xml_name, xml_item)) {
		output += 'red';
    } else if (xml_item.indexOf("\u00BD") > -1) { //½
		output += 'italic';
	} else if (xml_item.indexOf('Batik') > -1) {
		output += 'blue';
	} else if (xml_item.indexOf('Hijau') > -1) {
		output += 'green';
	} else if (xml_item.indexOf('Putih') > -1
			|| xml_item.indexOf('Marble') > -1) {
		output += 'white';
	}

	output += '"';
	output += '>';

	// Currently not remember after sorting
	output += '<td>';
	output += id + '.';
	output += '</td>';

	output += '<td>' + xml_soldto + '</td>';
	output += '<td>' + xml_order + '</td>';
	output += '<td>' + desc + '</td>';
	output += '<td>' + xml_item + '</td>';

	var chk = 'chk_';
	chk += id;
	output += '<td>';
	output += '<input type="checkbox" id="' + chk + '"';
	output += ' onclick="setStrikeThrough(\'' + chk + '\')"';
	output += ' style="cursor:pointer;" />'; // change cursor become a hand
	// when hover
	output += '</td>';

	// get aging value
	var aging = '';
	if (xml_order.length == 10) {

		if (xml_deliver.length == 10) {
			// deliver case
			var year = xml_order.substring(0, 4);
			var month = xml_order.substring(5, 7);
			var day = xml_order.substring(8, 10);
			var from = new Date();
			from.setFullYear(year, month - 1, day);

			var year = xml_deliver.substring(0, 4);
			var month = xml_deliver.substring(5, 7);
			var day = xml_deliver.substring(8, 10);
			var to = new Date();
			to.setFullYear(year, month - 1, day);

			aging = getDifferentDays(from, to);
		} else {
			// compare to now
			var year = xml_order.substring(0, 4);
			var month = xml_order.substring(5, 7);
			var day = xml_order.substring(8, 10);
			var from = new Date();
			from.setFullYear(year, month - 1, day);

			aging = getDifferentDays(from, new Date());
		}
	}
	output += '<td>';
	output += aging;
	output += '</td>';

	output += '</tr>';

	return output;
}

function parse(wrapper) {

    var date = new Date();
    var year = date.getYear() - 100 + 2000;

    $
			.ajax({
			    type: 'GET',
			    url: 'nisan.xml',
			    dataType: 'xml',
			    success: function (xml_list) {

			        var xmlArr = [];
			        var xml_no = 0; // numbering use
			        $(xml_list).find('order').each(function () {

			            var xml_deliver = $(this).attr('delivered');
			            if (xml_deliver == '') {
			                xml_no++;
			                xmlArr += generateCheckBoxRow(year, xml_no, this);
			            }
			        }); // end loop

			        // append array to table
			        $(xmlArr).appendTo(wrapper + ' table tbody');

			        // Add sort and zebra stripe to table (NOTE: this does not
			        // work as intended with sort feature)
			        window.setTimeout('$("' + wrapper
							+ ' table").tablesorter();', 120);
			        $(wrapper + ' table').hide().slideDown('200');

			        // display total item
			        $('#counter').text(xml_no);

			        // add filter by customer
			        var nav_link = $('#xml_nav li a');
			        nav_link
							.click(function () {

							    // get current index and reset all highlight
							    var currentClass = $(this).attr('class');
							    var currentIndex = -1;
							    nav_link
										.parent()
										.each(
												function (index) {
												    if ($(this).find('a').attr(
															'class') == currentClass)
												        currentIndex = index;
												    $(this).removeClass();
												});
							    // highlist selected value
							    nav_link.parent().each(function (index) {
							        if (index == currentIndex)
							            $(this).addClass('highlight');
							    });
							    // nav_link.parent().get(currentIndex).addClass('highlight');

							    xml_no = 0; // reset counter
							    var tr = wrapper + ' table tbody tr';
							    $(tr).show(); // show all rows
							    switch ($(this).attr('class')) {
							        case 'filter_adi':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('ADI') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_ham':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('HAM') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_ken':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('KEN') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_sel':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('SEL') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_sem':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('SEM') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;

							        default:
							            $(tr).filter(function (index) {
							                xml_no++;
							                return false;
							            }).hide();
							            break;
							    }

							    // display total item
							    $('#counter').text(xml_no);
							    // $(tr).removeClass('stripe');
							    // $(tr + ':visible:odd').addClass('stripe');
							}); // end click soldto

			        // add filter by month
			        var nav_month = $('#xml_month li a');
			        nav_month
							.click(function () {

							    // get current index and reset all highlight
							    var currentClass = $(this).attr('class');
							    var currentIndex = -1;
							    nav_month
										.parent()
										.each(
												function (index) {
												    if ($(this).find('a').attr(
															'class') == currentClass)
												        currentIndex = index;
												    $(this).removeClass();
												});
							    // highlist selected value
							    nav_month.parent().each(function (index) {
							        if (index == currentIndex)
							            $(this).addClass('highlight');
							    });

							    // retrieve filter soldto result set
							    $('table tbody').empty();

							    // get soldto
							    var soldto = '';
							    $('#xml_nav li').each(function (index) {
							        if ($(this).attr('class') == 'highlight')
							            soldto = $(this).find('a').text();
							    });
							    var array = parseXml(xml_list, soldto);
							    $(array).appendTo(wrapper + ' table tbody');
							    window.setTimeout('$("' + wrapper
										+ ' table").tablesorter();', 120);
							    $(wrapper + ' table').hide().slideDown('200');
							    // end filter soldto

							    xml_no = 0; // reset counter
							    var tr = wrapper + ' table tbody tr';
							    $(tr).show(); // show all rows
							    switch ($(this).attr('class')) {
							        case 'filter_01':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('01') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_02':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('02') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_03':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('03') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_04':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('04') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_05':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('05') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_06':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('06') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_07':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('07') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_08':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('08') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_09':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('09') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_10':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('10') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_11':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('11') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							        case 'filter_12':
							            $(tr).filter(
											function (index) {
											    var yes = ($(this).attr(
														'filterCriteria')
														.indexOf('12') >= 0);
											    if (yes) {
											        xml_no++;
											        $(this).find('td:first').text(xml_no + '.');
											    }
											    return !yes;
											}).hide();
							            break;
							    }

							    // display total item
							    $('#counter').text(xml_no);
							}); // end click on month
			    }
			});                      // end ajax
} // end function

/**
 * Return different from the date given in days. <code>
 *  var from = new Date();
 *  from.setFullYear(2011,10,1);
 *  var aging = getYTDAging(from);
 *  alert(aging+" days");
 * </code>
 * 
 * @param Date
 *            date Date value to compare
 * @deprecated
 */
function getYTDAging(from) {
	var now = new Date();
	var timestampNow = Math.round(now.getTime() / 1000);
	var timestampFrom = Math.round(from.getTime() / 1000);
	return Math.round((timestampNow - timestampFrom) / (60 * 60 * 24));
}
/**
 * Return different days from 2 date values.
 * 
 * @param Date
 *            from Start date value
 * @param Date
 *            to End date value
 */
function getDifferentDays(from, to) {
	var timestampTo = Math.round(to.getTime() / 1000);
	var timestampFrom = Math.round(from.getTime() / 1000);
	return Math.round((timestampTo - timestampFrom) / (60 * 60 * 24));
}

/**
 * Initialize page.
 */
$(function () {

    var wrapper = '#xml_wrapper';
    parse(wrapper);

    // this line fail use inline code instead see line#337
    // change cursor become a hand when hover
    // $(':checkbox').css('cursor', 'pointer');
});