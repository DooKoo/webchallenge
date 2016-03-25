// ***
// *** Morris Unobtrusive jQuery v0.4.3 (https://charts.codeplex.com)
// *** Copyright 2013 Daniel M. Porrey
// *** Licensed under https://charts.codeplex.com/license
// ***

// ***
// *** HTML to display when chart data cannot be loaded
// ***
var _ChartDataFailedDisplay = '<div class="alert alert-danger">Failed to load data</div>';

// ***
// *** Defines the chart types available
// ***
var chartTypes = {
	Area: { value: 0, name: "area" },
	Line: { value: 0, name: "line" },
	Donut: { value: 0, name: "donut" },
	Bar: { value: 0, name: "bar" },
};

// ***
// *** Defines chart option types that will drive
// *** how the options for the chart are set
// ***
var chartOptionTypes = {
	Id: { value: 0, name: "id" },
	String: { value: 1, name: "string" },
	Callback: { value: 2, name: "callback" },
	FormatCallback: { value: 3, name: "formatCallback" },
	Boolean: { value: 4, name: "boolean" },
	StringArray: { value: 5, name: "stringArray" },
	FloatArray: { value: 6, name: "floatArray" }
};

// ***
// *** Defines all of the possible options for the various charts, whether or not they are required, if the option
// *** should be automatically loaded (if detected), which chart supports the option type that will drive how the
// *** option is loaded or picked up from the data attributes.
// ***
var chartOptions =
	[
		{ name: 'element', isRequired: true, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar, chartTypes.Donut], optionType: chartOptionTypes.Id },
		{ name: 'dataUrl', isRequired: true, autoLoad: false, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar, chartTypes.Donut], optionType: chartOptionTypes.String },
		{ name: 'xkey', isRequired: true, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'ykeys', isRequired: true, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.StringArray },
		{ name: 'labels', isRequired: true, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.StringArray },
		{ name: 'lineColors', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'lineWidth', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'pointSize', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'pointFillColors', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'pointStrokeColors', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'ymax', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'ymin', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'smooth', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.Boolean },
		{ name: 'hideHover', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'hoverCallback', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.Callback },
		{ name: 'parseTime', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.Boolean },
		{ name: 'units', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'postUnits', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'preUnits', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'dateFormat', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.FormatCallback },
		{ name: 'xLabels', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'xLabelFormat', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.FormatCallback },
		{ name: 'yLabelFormat', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.FormatCallback },
		{ name: 'goals', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.FloatArray },
		{ name: 'goalStrokeWidth', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'goalLineColors', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'events', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'eventStrokeWidth', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'eventLineColors', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.StringArray },
		{ name: 'continuousLine', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'axes', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.Boolean },
		{ name: 'grid', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.Boolean },
		{ name: 'gridTextColor', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'gridTextSize', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'gridTextFamily', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'gridTextWeight', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line, chartTypes.Bar], optionType: chartOptionTypes.String },
		{ name: 'fillOpacity', isRequired: false, autoLoad: true, chart: [chartTypes.Area, chartTypes.Line], optionType: chartOptionTypes.String },
		{ name: 'behaveLikeLine', isRequired: false, autoLoad: true, chart: [chartTypes.Area], optionType: chartOptionTypes.String },
		{ name: 'barColors', isRequired: false, autoLoad: true, chart: [chartTypes.Bar], optionType: chartOptionTypes.StringArray },
		{ name: 'stacked', isRequired: false, autoLoad: true, chart: [chartTypes.Bar], optionType: chartOptionTypes.Boolean },
		{ name: 'colors', isRequired: false, autoLoad: true, chart: [chartTypes.Donut], optionType: chartOptionTypes.StringArray },
		{ name: 'formatter', isRequired: false, autoLoad: true, chart: [chartTypes.Donut], optionType: chartOptionTypes.FormatCallback },
		{ name: 'labelColor', isRequired: false, autoLoad: true, chart: [chartTypes.Donut], optionType: chartOptionTypes.String },
		{ name: 'backgroundColor', isRequired: false, autoLoad: true, chart: [chartTypes.Donut], optionType: chartOptionTypes.String }
	];

// ***
// *** Defines the formatter functions. More functions can be added to
// *** create custom formatters. The format attribute is defined as
// *** attributeName="formatterType(formatString)". For example, the
// *** the xLabelFormat for a Line Chart is defined as 
// *** data-chart-xLabelFormat="date(MMMM, YYYY)" where the text 'date'
// *** matches one of the defined formats. The parameter 'value' is passed
// *** by the Morris.js chart library to the function.
// ***
var formatters =
	{
		date: function (value, formatString) { return new moment(value).format(formatString); },
		append: function (value, formatString) { return value + formatString; },
		prepend: function (value, formatString) { return formatString + value; }
	};

// ***
// ***
// ***
$(function () {
	// ***
	// *** Attach to the resize event. Debounce will ensure this is called once
	// *** after the window stops resizing.
	// ***
	$(window).resize($.debounce(250, true, function (e) {
		initializeCharts();
	}));

	// ***
	// *** Initialize all charts
	// ***
	initializeCharts();
});

// ***
// *** Find all HTML elements that have a data-chart
// *** element defined and initialize them.
// ***
function initializeCharts() {
	// ***
	// *** Find all charts
	// ***
	$('div[data-chart-type]').each(function () {
		// ***
		// *** Get a reference to the HTML element where
		// *** the chart is to be created
		// ***
		var htmlElement = $(this);

		// ***
		// *** Get the chart type name from the attribute
		// ***
		var chartTypeName = htmlElement.attr('data-chart-type');

		// ***
		// *** Convert the chart type name to a chartType
		// ***
		var chartType = getChartTypeByName(chartTypeName);

		// ***
		// *** If chartType is null then an invalid chart type
		// *** was specified.
		// ***
		if (chartType !== null) {
			if (chartType !== null) {
				// ***
				// *** Initialize the HTML element
				// ***
				initializeHtmlElement(htmlElement);

				// ***
				// *** Create a chart of the specified type
				// ***
				var chart = createChart(chartType, htmlElement);
			}
		}
		else {
			// ***
			// *** Throw an exception
			// ***
			jQuery.error('The chart type "' + chartTypeName + '" is invalid.');
		}
	});
}

// ***
// *** Given the name of a chart type it returns
// *** a chartType object.
// ***
function getChartTypeByName(chartTypeName) {
	var returnValue = "";

	switch (chartTypeName) {
		case chartTypes.Area.name:
			returnValue = chartTypes.Area;
			break;
		case chartTypes.Line.name:
			returnValue = chartTypes.Line;
			break;
		case chartTypes.Donut.name:
			returnValue = chartTypes.Donut;
			break;
		case chartTypes.Bar.name:
			returnValue = chartTypes.Bar;
			break;
	}

	return returnValue;
}

// ***
// *** Performs necessary initialization on the HTML element
// ***
function initializeHtmlElement(htmlElement) {
	// ***
	// *** Using jQuery to select the chart div by id. Remove all
	// *** child nodes of the set of matched elements from the DOM.
	// ***
	htmlElement.empty();
}

// ***
// *** Creates the specified chart type
// ***
function createChart(chartType, htmlElement) {
	var chart = null;
	var optionsContainer = createOptions(htmlElement, chartType);
	var dataContainer = getDataChartAttributeValue(htmlElement, 'dataUrl', false);

	switch (chartType) {
		case chartTypes.Area:
			$.getJSON(dataContainer, function (json) {
				optionsContainer.options.data = json;
				chart = new Morris.Area(optionsContainer.options);
			}).fail(function () {
				htmlElement.html(_ChartDataFailedDisplay);
			});
			break;
		case chartTypes.Line:
			$.getJSON(dataContainer, function (json) {
				optionsContainer.options.data = json;
				chart = new Morris.Line(optionsContainer.options);
			}).fail(function () {
				htmlElement.html(_ChartDataFailedDisplay);
			});
			break;
		case chartTypes.Bar:
			$.getJSON(dataContainer, function (json) {
				optionsContainer.options.data = json;
				chart = new Morris.Bar(optionsContainer.options);
			}).fail(function () {
				htmlElement.html(_ChartDataFailedDisplay);
			});
			break;
		case chartTypes.Donut:
			$.getJSON(dataContainer, function (json) {
				optionsContainer.options.data = json;
				chart = new Morris.Donut(optionsContainer.options);
			}).fail(function () {
				htmlElement.html(_ChartDataFailedDisplay);
			});
			break;
	}

	return chart;
}

// ***
// *** Creates the options array and populates it for a chart
// ***
function createOptions(htmlElement, chartType) {
	// ***
	// *** Create an array that will contain the options for a chart
	// ***
	var chart = { chartType: chartType, options: {} };

	// ***
	// *** Set the chart options
	// ***
	for (var i = 0; i < chartOptions.length; i++) {
		// ***
		// *** Setting all properties that are not required
		// ***
		if (chartOptions[i].chart.indexOf(chart.chartType) >= 0) {
			// ***
			// *** Only set those marked as autoLoad = true
			// ***
			if (chartOptions[i].autoLoad) {
				switch (chartOptions[i].optionType) {
					case chartOptionTypes.Id:
						// ***
						// *** The attribute value contains a string value
						// ***
						setChartElementOption(htmlElement, chart);
						break;
					case chartOptionTypes.Boolean:
						// ***
						// *** The attribute value contains a string value
						// ***
						setDataChartBooleanOption(htmlElement, chart, chartOptions[i].name, chartOptions[i].isRequired);
						break;
					case chartOptionTypes.String:
						// ***
						// *** The attribute value contains a string value
						// ***
						setDataChartOption(htmlElement, chart, chartOptions[i].name, chartOptions[i].isRequired);
						break;
					case chartOptionTypes.FormatCallback:
						// ***
						// *** The attribute value contains a format string
						// ***
						setFormatCallbackOption(htmlElement, chart, chartOptions[i].name, chartOptions[i].isRequired);
						break;
					case chartOptionTypes.StringArray:
						// ***
						// *** The attribute value contains a format string
						// ***
						setStringArrayDataChartOption(htmlElement, chart, chartOptions[i].name, chartOptions[i].isRequired);
						break;
					case chartOptionTypes.FloatArray:
						// ***
						// *** The attribute value contains a format string
						// ***
						setFloatArrayDataChartOption(htmlElement, chart, chartOptions[i].name, chartOptions[i].isRequired);
						break;
				}
			}
		}
		else {
			if (isDefined(htmlElement, chartOptions[i].name)) {
				// ***
				// *** Throw an exception
				// ***
				jQuery.error('The chart option "' + chartOptions[i].name + '" is invalid for a "' + chart.chartType.name + '" chart.');
			}
		}
	}

	return chart;
}

// ***
// *** Determines if a specific attribute exists
// *** on the HTML element.
// ***
function isDefined(htmlElement, attributeName) {
	// ***
	// *** Determine if the element exists
	// ***
	var value = htmlElement.attr('data-chart-' + attributeName);

	if (value == '' || value == null) {
		return false;
	}
	else {
		return true;
	}
}

// ***
// *** Sets a chart option by name by retrieving the chart option
// *** value from the associated attribute.
// ***
function setChartElementOption(htmlElement, chart) {
	var value = htmlElement.attr("Id");

	if (value != '' && value != null) {
		chart.options.element = getChartAttributeValue(htmlElement, "Id", true);
	}
}

// ***
// *** Sets a chart option by name by retrieving the chart option
// *** value from the associated data-chart attribute and converts
// *** it to a boolean (expected values are "true" or "false").
// ***
function setDataChartBooleanOption(htmlElement, chart, optionName, isRequired) {
	if (isDefined(htmlElement, optionName)) {
		chart.options[optionName] = (getDataChartAttributeValue(htmlElement, optionName, isRequired, "false") == "true");
	}
}

// ***
// *** Sets a chart option by name by retrieving the chart option
// *** value from the associated data-chart attribute.
// ***
function setDataChartOption(htmlElement, chart, optionName, isRequired) {
	if (isDefined(htmlElement, optionName)) {
		chart.options[optionName] = getDataChartAttributeValue(htmlElement, optionName, isRequired);
	}
}

// ***
// *** Sets a chart option by name by retrieving the chart option
// *** value from the associated data-chart attribute.
// ***
function setStringArrayDataChartOption(htmlElement, chart, optionName, isRequired) {
	if (isDefined(htmlElement, optionName)) {
		var arrayLine = getDataChartAttributeValue(htmlElement, optionName, isRequired);
		var items = arrayLine.split(",");
		chart.options[optionName] = items;
	}
}

// ***
// *** Sets a chart option by name by retrieving the chart option
// *** value from the associated data-chart attribute.
// ***
function setFloatArrayDataChartOption(htmlElement, chart, optionName, isRequired) {
	if (isDefined(htmlElement, optionName)) {
		var arrayLine = getDataChartAttributeValue(htmlElement, optionName, isRequired);
		var items = arrayLine.split(",");
		chart.options[optionName] = items;

		for (var a in chart.options[optionName]) {
			chart.options[optionName][a] = parseFloat(chart.options[optionName][a]);
		}
	}
}

// ***
// *** Sets a formatter function for options that require a callback (function)
// *** to format data.
// ***
function setFormatCallbackOption(htmlElement, chart, optionName, isRequired) {
	if (isDefined(htmlElement, optionName)) {
		// ***
		// *** Get the value that contains the formatter type and the format string
		// *** which is in the format 'formatterType[formatString]'
		// ***
		var attributeValue = getDataChartAttributeValue(htmlElement, optionName, false);
		var expression = /\s*\((.+?)\)/;

		// ***
		// *** Parse the formatter
		// ***
		if (expression.test(attributeValue)) {
			// ***
			// *** Parse the string
			// ***
			var parsed = attributeValue.split(expression);

			// ***
			// *** The formatter type will choose the function
			// *** to call.
			// ***
			var formatterType = parsed[0];

			// ***
			// *** The formatString is passed to the function
			// ***
			var formatString = parsed[1];

			// ***
			// *** Choose the correct formatter by type
			// ***
			if (formatters[formatterType] == undefined) {
				// ***
				// *** Assign the callback to the chart option
				// ***
				jQuery.error('The formatter type "' + formatterType + '" is not a recognized callback formatter.');
			}
			else {
				// ***
				// *** There is no formatter function defined for this type
				// ***
				chart.options[optionName] = function (value) { return formatters[formatterType](value, formatString); };
			}
		}
		else {
			// ***
			// *** The entire attribute value was not recognized (could not be parsed)
			// ***
			jQuery.error('The formatter text "' + attributeValue + '" is not recognized.');
		}
	}
}

// ***
// *** Gets the value of a specific attribute
// ***
function getChartAttributeValue(htmlElement, attributeName, isRequired) {
	// ***
	// *** Determine if the element exists
	// ***
	var value = htmlElement.attr(attributeName);

	if (value == '' || value == null) {
		if (isRequired) {
			jQuery.error('The element "' + attributeName + '" is required.');
		}
	}
	else {
		return value;
	}
}

// ***
// *** Gets the value of a specific data-chart-* attribute
// ***
function getDataChartAttributeValue(htmlElement, attributeName, isRequired) {
	return getChartAttributeValue(htmlElement, 'data-chart-' + attributeName, isRequired);
}