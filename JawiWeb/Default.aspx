<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nisan Report</title>
    <link href="Style.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel1" runat="server" CssClass="filter" GroupingText="Search">
        <asp:Label ID="Label1" runat="server" CssClass="labeling" Text="Customer:"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="entry"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="Status:" CssClass="entry"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="entry">
            <asp:ListItem Selected="True">Pending</asp:ListItem>
            <asp:ListItem>Delivered</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label2" runat="server" CssClass="labeling" Text="From:"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" CssClass="entry"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" CssClass="entry" Text="To:"></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="entry"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="View" CssClass="button" />
    </asp:Panel>
    <div class="content">
        <asp:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        </asp:ReportViewer>
    </div>
    <div class="footer">
        <hr />
        600 & 601 Taman Saga II, Jalan Alor Mengkudu, 05400 Alor Star, Kedah, Malaysia |
        <a href="mailto:hlgranite@gmail.com">Contact Us</a>
        <br />
        mobile: +6019-474 5377, +6012-431 9377, +6012-4200 963
        <br />
        office: +604-7306 991 Fax: +604-7306 991
        <br />
        &copy;Copyright 2008 HL Granite Manufacturing
    </div>
    </form>
</body>
</html>
