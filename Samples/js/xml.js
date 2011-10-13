/*
jQuery XML Parser with Sorting and Filtering
Written by Ben Lister (darkcrimson.com) revised 25 Apr 2010
Tutorial: http://blog.darkcrimson.com/2010/01/jquery-xml-parser/

Licensed under the MIT License:
http://www.opensource.org/licenses/mit-license.php
*/

/**
 @todo: add validation rule to highlight in red.
*/
$(function () {

    function xml_parser(wrapper) {
        $.ajax({
            type: 'GET',
            url: 'nisan.xml',
            dataType: 'xml',
            success: function (xml_list) {

                var xmlArr = [];
                var xml_no = 0; //numbering use
                $(xml_list).find('order').each(function () {

                    var xml_deliver = $(this).attr('delivered');
                    if (xml_deliver == '') {

                        xml_no++;
                        //property fields
                        var xml_soldto = $(this).attr('soldto');
                        var xml_order = $(this).attr('date');
                        var xml_price = $(this).attr('price');

                        //@todo: romanize the name letter
                        var xml_name = $(this).find('name').text();
                        var xml_jawi = $(this).find('jawi').text();
                        var xml_born = $(this).find('born').text();
                        var xml_death = $(this).find('death').text();
                        var xml_deathm = $(this).find('deathm').text();
                        var xml_age = $(this).find('age').text();
                        var xml_remarks = $(this).find('remarks').text();
                        var xml_item = $(this).find('item').text();

                        //compute desciption
                        var desc = '';
                        desc += xml_name;
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
                        desc += xml_jawi;


                        //able to filter upon soldto person and month only
                        xmlArr += '<tr filterCriteria="';
                        xmlArr += xml_soldto;
                        xmlArr += xml_order.substring(5, 7); //extract only month value
                        xmlArr += '"';

                        //add color style
                        xmlArr += ' class="';
                        if (xml_item.indexOf('Batik') >= 0) {
                            xmlArr += 'blue';
                        } else if (xml_item.indexOf('Hijau') >= 0) {
                            xmlArr += 'green';
                        } else if (xml_item.indexOf('Putih') >= 0) {
                            xmlArr += 'white';
                        } else if (xml_item.indexOf('½') >= 0) {
                            xmlArr += 'italic';
                        }
                        xmlArr += '"';

                        xmlArr += '>';

                        //@todo: correct the numbering. Currently not renumber after sorting
                        xmlArr += '<td>';
                        xmlArr += ''; // xml_no;
                        xmlArr += '</td>';

                        xmlArr += '<td>';
                        xmlArr += xml_soldto;
                        xmlArr += '</td>';

                        xmlArr += '<td>';
                        xmlArr += xml_order;
                        xmlArr += '</td>';

                        xmlArr += '<td>';
                        xmlArr += desc;
                        xmlArr += '</td>';

                        xmlArr += '<td>';
                        xmlArr += xml_item;
                        xmlArr += '</td>';

                        xmlArr += '</tr>';
                    }
                }); //end loop

                //append array to table
                $(xmlArr).appendTo(wrapper + ' table tbody');

                //Add sort and zebra stripe to table (NOTE: this does not work as intended with sort feature)
                window.setTimeout('$("' + wrapper + ' table").tablesorter();', 120);
                $(wrapper + ' table').hide().slideDown('200');

                //add filter by customer
                var nav_link = $('#xml_nav li a');
                nav_link.click(function () {
                    var tr = wrapper + ' table tbody tr';
                    $(tr).show(); //show all rows
                    switch ($(this).attr('class')) {
                        case 'filter_adi':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('ADI') >= 0);
                            }).hide();
                            break;
                        case 'filter_ham':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('HAM') >= 0);
                            }).hide();
                            break;
                        case 'filter_ken':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('KEN') >= 0);
                            }).hide();
                            break;
                        case 'filter_sel':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('SEL') >= 0);
                            }).hide();
                            break;
                        case 'filter_sem':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('SEM') >= 0);
                            }).hide();
                            break;

                        default:
                            $(tr).filter(function (index) {
                                return false;
                            }).hide();
                            break;
                    }

                    //$(tr).removeClass('stripe');
                    //$(tr + ':visible:odd').addClass('stripe');
                }); //end click

                //add filter by month
                var nav_month = $('#xml_month li a');
                nav_month.click(function () {
                    var tr = wrapper + ' table tbody tr';
                    $(tr).show(); //show all rows
                    switch ($(this).attr('class')) {
                        case 'filter_01':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('01') >= 0);
                            }).hide();
                            break;
                        case 'filter_02':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('02') >= 0);
                            }).hide();
                            break;
                        case 'filter_03':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('03') >= 0);
                            }).hide();
                            break;
                        case 'filter_04':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('04') >= 0);
                            }).hide();
                            break;
                        case 'filter_05':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('05') >= 0);
                            }).hide();
                            break;
                        case 'filter_06':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('06') >= 0);
                            }).hide();
                            break;
                        case 'filter_07':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('07') >= 0);
                            }).hide();
                            break;
                        case 'filter_08':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('08') >= 0);
                            }).hide();
                            break;
                        case 'filter_09':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('09') >= 0);
                            }).hide();
                            break;
                        case 'filter_10':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('10') >= 0);
                            }).hide();
                            break;
                        case 'filter_11':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('11') >= 0);
                            }).hide();
                            break;
                        case 'filter_12':
                            $(tr).filter(function (index) {
                                return !($(this).attr('filterCriteria').indexOf('12') >= 0);
                            }).hide();
                            break;
                    }
                });

            }
        }); //end ajax
    } //end function

    //initialize page
    var wrapper = '#xml_wrapper';
    xml_parser(wrapper);

});