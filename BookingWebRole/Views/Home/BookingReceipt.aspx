<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Ploeh.Samples.Booking.WebModel.BookingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    BookingReceipt
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Thank your for your reservation!</h2>
    <fieldset>
        <legend>We have recorded the following about your request:</legend>
        <div class="display-label">
            Date</div>
        <div class="display-field">
            <%: this.Model.Date.ToShortDateString() %></div>
        <div class="display-label">
            Name</div>
        <div class="display-field">
            <%: this.Model.Name %></div>
        <div class="display-label">
            Email
        </div>
        <div class="display-field">
            <%: this.Model.Email %>
        </div>
        <div class="display-label">
            Quantity</div>
        <div class="display-field">
            <%: this.Model.Quantity %></div>
    </fieldset>
    <p>
        Please note that this page only verifies that we have received your request. It
        does not guarantee that we can fulfill your reservation, but in either case we will
        send you an email as soon as we know.
    </p>
    <p>
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>
