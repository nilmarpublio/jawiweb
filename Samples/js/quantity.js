/**
 * Count on quantity through the 12 months plus append total at the end of row.
 */
var total = [0,0,0,0,0,0,0,0,0,0,0,0,0];
var stickers = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#71c73e
var pv = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#944117
var nisans = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#77b7c5
var rehals = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#E47C47
var etc = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#d2ec72
	
/**
 * Process data into months dimension.
 */
function processData(year) {
	
	console.log("processData("+year+")");
	
	//reset
	total = [0,0,0,0,0,0,0,0,0,0,0,0,0];
	stickers = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#71c73e
	pv = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#944117
	nisans = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#77b7c5
	rehals = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#E47C47
	etc = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#d2ec72
	
	$.ajax({
		type: "GET",
		url: "nisan.xml",
		dataType: "xml",
		success: function(xml) {
			console.log("start manipulate each order");
			$(xml).find("order").each(function() {
				var date = $(this).attr("date");
				var	month = parseFloat(date.substring(5,7));
				var stock = $(this).find('item').text();
				if(date.substring(0,4) == year) {
				    total[month - 1] += 1;
				    total[12] += 1;
					if(stock.toLowerCase().indexOf('sticker') > -1) {
						stickers[month-1] += 1;
						stickers[stickers.length-1] += 1;
					} else if(stock.toLowerCase().indexOf('pv') > -1) {
						pv[month-1] += 1;
						pv[pv.length-1] += 1;
					} else if(stock.toLowerCase().indexOf('batu') > -1) {
						nisans[month-1] += 1;
						nisans[nisans.length-1] += 1;
					} else if(stock.toLowerCase().indexOf('rehal') > -1) {
						rehals[month-1] += 1;
						rehals[rehals.length-1] += 1;
					} else {
						etc[month-1] += 1;
						etc[etc.length-1] += 1;
					}
				}
			});
			
			showTable();
			showGraph();
		}
	});
}

/**
 * Display monthly quantity sold for each stock type.
 */
function showTable() {
	$("#results").empty();
	
	var row = "<tr>";
	row += "<td></td><td>Jan</td><td>Feb</td><td>Mar</td><td>Apr</td><td>May</td><td>Jun</td><td>Jul</td><td>Aug</td><td>Sep</td><td>Oct</td><td>Nov</td><td>Dec</td><td>Total</td>";
	row += "</tr>";
	$("#results").append(row);
	
	row = "<tr><td>Sticker</td>";
	for(var i=0;i<stickers.length;i++)
	 row += "<td>"+stickers[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	row = "<tr><td>PV</td>";
	for(var i=0;i<pv.length;i++)
	 row += "<td>"+pv[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);	
	
	row = "<tr><td>Nisan</td>";
	for(var i=0;i<nisans.length;i++)
	 row += "<td>"+nisans[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);	
	
	row = "<tr><td>Rehal</td>";
	for(var i=0;i<rehals.length;i++)
	 row += "<td>"+rehals[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	row = "<tr><td>etc</td>";
	for(var i=0;i<etc.length;i++)
		row += "<td>"+etc[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	
	row = "<tr style='font-weight:bold;'><td>Total</td>";
	for(var i=0;i<total.length;i++)
		row += "<td>"+total[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
}

/**
 * Display quantity sold in graph.
 * @source http://designmodo.com/create-interactive-graph-css3-jquery/
 */
function showGraph() {
	//y axis must be in number. fail after change to "Jan"
  	var hpoints = [];
  	for(var i=0;i<stickers.length-1;i++) {
  		hpoints[i] = [i+1, stickers[i]];
  	}
  	
  	var apoints = [];
  	for(var i=0;i<pv.length-1;i++) {
  		apoints[i] = [i+1, pv[i]];
  	}
  	//alert("ADI: "+apoints);
  	
  	var spoints = [];
  	for(var i=0;i<nisans.length-1;i++) {
  		spoints[i] = [i+1, nisans[i]];
  	}
  	//alert("SEL: "+spoints);
  	
  	var mpoints = [];
  	for(var i=0;i<rehals.length-1;i++) {
  		mpoints[i] = [i+1, rehals[i]];
  	}
  	//alert("SEM: "+mpoints);
  	
  	var epoints = [];
  	for(var i=0;i<etc.length-1;i++) {
  		epoints[i] = [i+1, etc[i]];
  	}
  	//alert("other: "+epoints);
  	
	var graphData = [
		{
	        data: hpoints,
	        color: '#71c73e',
	        points: { radius: 4, fillColor: '#71c73e' }
	    },
	    {
	        data: apoints,
	        color: '#944117',
	        points: { radius: 4, fillColor: '#944117' }
        },
		{
	        data: spoints,
	        color: '#77b7c5',
	        points: { radius: 4, fillColor: '#77b7c5' }
	    },
		{
	        data: mpoints,
	        color: '#E47C47',
	        points: { radius: 4, fillColor: '#E47C47' }
		},
		{
	        data: epoints,
	        color: '#d2ec72',
	        points: { radius: 4, fillColor: '#d2ec72' }
	    }
	];
	
	//plot the lines graph
	$.plot($("#graph-lines"), graphData, {
		series: {
			points: {show:true, radius:5},
			lines: {show:true},
			shadowSize: 0
		},
		grid: {color: '#646464', borderColor:'transparent', borderWidth: 20, hoverable: true},
		xaxis: {tickColor: 'transparent'},
		yaxis: {tickSize: 50},
	});
	
	//plot stack-bar graph
	//@see http://www.saltycrane.com/blog/2010/03/jquery-flot-stacked-bar-chart-example/
	$.plot($("#graph-bars"),graphData,{
		series: {
			stack: 0,
			lines: {show: false, steps: false},
			bars: {show:true, barWidth:.9, align:"center"},
			shadowSize:0,
		},
		grid: {color:"#646464", borderColor:"transparent", borderWidth:20, hoverable:true},
		xaxis: {tickColor:'transparent'},
		yaxis: {tickSize:50},
	});
}

/**
 * Convert to readable month string.
 * @param int i 1-based value.
 */
function convertToMonth(i) {

	switch(i%12) {
		case 0: return "Dec"; break;
		case 1: return "Jan"; break;
		case 2: return "Feb"; break;
		case 3: return "Mar"; break;
		case 4: return "Apr"; break;
		case 5: return "May"; break;
		case 6: return "Jun"; break;
		case 7: return "Jul"; break;
		case 8: return "Aug"; break;
		case 9: return "Sep"; break;
		case 10: return "Oct"; break;
		case 11: return "Nov"; break;		
	}
}


$(function() {
	
	/* toggle graph */
	$('#graph-bars').hide(); 
	$('#lines').on('click', function (e) {
	    $('#bars').removeClass('active');
	    $('#graph-bars').fadeOut();
	    $(this).addClass('active');
	    $('#graph-lines').fadeIn();
	    e.preventDefault();
	});
	 
	$('#bars').on('click', function (e) {
	    $('#lines').removeClass('active');
	    $('#graph-lines').fadeOut();
	    $(this).addClass('active');
	    $('#graph-bars').fadeIn().removeClass('hidden');
	    e.preventDefault();
	});
	
	var today = new Date();
	var year = today.getYear()+1900;
	$("#year").val(year);
	processData(year);
	
	/**
	 * Show tooltip
	 */
	function showTooltip(x, y, contents) {
	    $('<div id="tooltip">' + contents + '</div>').css({
	        top: y - 16,
	        left: x + 20
	    }).appendTo('body').fadeIn();
	}
	 
	var previousPoint = null;	 
	$('#graph-lines, #graph-bars').bind('plothover', function (event, pos, item) {
	    if (item) {
	        if (previousPoint != item.dataIndex) {
	            previousPoint = item.dataIndex;
	            $('#tooltip').remove();
	            var x = item.datapoint[0],
	                y = item.datapoint[1];
	                showTooltip(item.pageX, item.pageY, y + ' at ' + convertToMonth(x));
	        }
	    } else {
	        $('#tooltip').remove();
	        previousPoint = null;
	    }
	});
	
	/**
	 * Year selection onchange event.
	 */
	$("#year").change(function() {
		processData($(this).val());
	});

});