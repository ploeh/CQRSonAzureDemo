<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BookingViewModel>" %>
<%@ Import Namespace="Ploeh.Samples.Booking.WebModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Make a reservation
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Make a reservation</h2>
    <%: this.Html.EditorForModel() %>
</asp:Content>
