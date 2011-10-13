/*
jQuery XML Parser with Sorting and Filtering
Written by Ben Lister (darkcrimson.com) revised 25 Apr 2010
Tutorial: http://blog.darkcrimson.com/2010/01/jquery-xml-parser/

Licensed under the MIT License:
http://www.opensource.org/licenses/mit-license.php
*/

$(function () {

    function xml_parser(wrapper) {
        $.ajax({
            type: 'GET',
            url: 'nisan.xml',
            dataType: 'xml',
            success: function (xml_list) {

                var xmlArr = [];
                $(xml_list).find('order').each(function () {

                    var xml_deliver = $(this).attr('delivered');
                    if (xml_deliver == '') {

                        //property fields
                        var xml_soldto = $(this).attr('soldto');
                        var xml_order = $(this).attr('date');
                        var xml_price = $(this).attr('price');

                        var xml_name = $(this).find('name').text();//@todo: romanize the name letter
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


                        xmlArr += '<tr>';

                        xmlArr += '<td>';
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
            }
        }); //end ajax
    } //end function

    //initialize page
    var wrapper = '#xml_wrapper';
    xml_parser(wrapper);

});