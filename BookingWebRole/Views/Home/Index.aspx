<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<string>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Select a date
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.ui.core.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.ui.widget.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.ui.datepicker.min.js"></script>
    <script type="text/javascript">
        $(function () {

            var disabledDays =[<% foreach (var date in this.Model)	{ %> "<%: date %>",	<% } %>];

            $("#datepicker").datepicker({
                beforeShowDay: getStatusForDate,
                dateFormat: 'yy-mm-dd',
                onChangeMonthYear: changeMonthYear,
                onSelect: selectDate
            });

            function changeMonthYear(year, month, inst) {
                $.getJSON("<%: this.Url.Action("DisabledDays") %>", { year: year, month: month }, disableDates);
            }

            function selectDate(dateText, inst) {
                window.location.href = "<%: this.Url.Action("NewBooking") %>/" + dateText;
            }

            function disableDates(dates) {
                disabledDays = dates;
                $("#datepicker").datepicker("refresh");
            }

            function getStatusForDate(date) {
                var dateFormat = $("#datepicker").datepicker("option", "dateFormat");
                var formattedDate = $.datepicker.formatDate(dateFormat, date);
                if ($.inArray(formattedDate, disabledDays) != -1) {
                    return [false];
                }
                return [true];
            }
        });
    </script>
    <h2>
        Please select a date</h2>
    <div id="datepicker">
    </div>
</asp:Content>
