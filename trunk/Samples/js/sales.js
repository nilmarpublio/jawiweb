/**
 * Sales array through the 12 months plus append total at the end of row.
 */
var sales = [0,0,0,0,0,0,0,0,0,0,0,0,0];

var hamids = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#71c73e
var addins = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#944117
var selamas = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#77b7c5
var semanggols = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#E47C47
var etc = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#d2ec72
	
/**
 * Process data into months dimension.
 */
function processData(year) {
	
	console.log("processData("+year+")");
	
	//reset
	sales = [0,0,0,0,0,0,0,0,0,0,0,0,0];
	hamids = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#71c73e
	addins = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#944117
	selamas = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#77b7c5
	semanggols = [0,0,0,0,0,0,0,0,0,0,0,0,0];//#E47C47
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
				var customer = $(this).attr("soldto");
				if(date.substring(0,4) == year) {
				    sales[month - 1] += parseFloat($(this).attr("price"));
				    sales[12] += parseFloat($(this).attr("price"));
					switch(customer) {
						case "HAM":
						    hamids[month - 1] += parseFloat($(this).attr("price"));
						    hamids[12] += parseFloat($(this).attr("price"));
						break;
						
						case "ADI":
						    addins[month - 1] += parseFloat($(this).attr("price"));
						    addins[12] += parseFloat($(this).attr("price"));
						break;
						
						case "SEL":
						    selamas[month - 1] += parseFloat($(this).attr("price"));
						    selamas[12] += parseFloat($(this).attr("price"));
						break;
						
						case "SEM":
						    semanggols[month - 1] += parseFloat($(this).attr("price"));
						    semanggols[12] += parseFloat($(this).attr("price"));
						break;
						
						default:
						    etc[month - 1] += parseFloat($(this).attr("price"));
						    etc[12] += parseFloat($(this).attr("price"));
						break;
					}
				}
			});
			
			showTable();
			showGraph();
		}
	});
}

/**
 * Display monthly sales for each customer.
 */
function showTable() {
	$("#results").empty();
	
	var row = "<tr>";
	row += "<td></td><td>Jan</td><td>Feb</td><td>Mar</td><td>Apr</td><td>May</td><td>Jun</td><td>Jul</td><td>Aug</td><td>Sep</td><td>Oct</td><td>Nov</td><td>Dec</td><td>Total</td>";
	row += "</tr>";
	$("#results").append(row);	
	
	
	row = "<tr><td>ADI</td>";
	for(var i=0;i<addins.length;i++)
	 row += "<td>"+addins[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	
	row = "<tr><td>SEM</td>";
	for(var i=0;i<semanggols.length;i++)
	 row += "<td>"+semanggols[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	
	row = "<tr><td>SEL</td>";
	for(var i=0;i<selamas.length;i++)
	 row += "<td>"+selamas[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);	
	
	
	row = "<tr><td>HAM</td>";
	for(var i=0;i<hamids.length;i++)
	 row += "<td>"+hamids[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	row = "<tr><td>etc</td>";
	for(var i=0;i<etc.length;i++)
		row += "<td>"+etc[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
	
	
	row = "<tr style='font-weight:bold;'><td>Total</td>";
	for(var i=0;i<sales.length;i++)
		row += "<td>"+sales[i]+"</td>";
	row += "</tr>";
	$("#results").append(row);
}

/**
 * Display sales in graph.
 * @source http://designmodo.com/create-interactive-graph-css3-jquery/
 */
function showGraph() {
	//y axis must be in number. fail after change to "Jan"
  	var hpoints = [];
  	for(var i=0;i<hamids.length-1;i++) {
  		hpoints[i] = [i+1, hamids[i]];
  	}
  	//alert("HAM: "+hpoints);
  	
  	var apoints = [];
  	for(var i=0;i<addins.length-1;i++) {
  		apoints[i] = [i+1, addins[i]];
  	}
  	//alert("ADI: "+apoints);
  	
  	var spoints = [];
  	for(var i=0;i<selamas.length-1;i++) {
  		spoints[i] = [i+1, selamas[i]];
  	}
  	//alert("SEL: "+spoints);
  	
  	var mpoints = [];
  	for(var i=0;i<semanggols.length-1;i++) {
  		mpoints[i] = [i+1, semanggols[i]];
  	}
  	//alert("SEM: "+mpoints);
  	
  	var epoints = [];
  	for(var i=0;i<etc.length-1;i++) {
  		epoints[i] = [i+1, etc[i]];
  	}
  	//alert("other: "+epoints);
  	
	var graphData = [{
	        data: apoints,
	        color: '#944117',
	        points: { radius: 4, fillColor: '#944117' }
      }, {
	        data: mpoints,
	        color: '#E47C47',
	        points: { radius: 4, fillColor: '#E47C47' }
      }, {
	        data: spoints,
	        color: '#77b7c5',
	        points: { radius: 4, fillColor: '#77b7c5' }
	    }, {
	        data: hpoints,
	        color: '#71c73e',
	        points: { radius: 4, fillColor: '#71c73e' }
	    }, {
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
		yaxis: {tickSize: 500},
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
		yaxis: {tickSize:500},
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