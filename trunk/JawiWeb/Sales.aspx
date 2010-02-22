<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Sales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nisan Sales</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Panel ID="Panel1" runat="server" GroupingText="Selection">
            <asp:Label ID="Label4" runat="server" Text="Report Type:"></asp:Label>
            <asp:DropDownList ID="ddlReportType" runat="server">
                <asp:ListItem Selected="True" Value="NisanDetails.rdlc">Details</asp:ListItem>
                <asp:ListItem Value="NisanByMonth.rdlc">Monthly</asp:ListItem>
                <asp:ListItem Value="NisanByMonthSummary.rdlc">Month Summary</asp:ListItem>
                <asp:ListItem Value="NisanSummary.rdlc">Summary</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="Label3" runat="server" Text="Customer:"></asp:Label>
            <asp:DropDownList ID="ddlCustomer" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label1" runat="server" Text="From:"></asp:Label>
            <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="To:"></asp:Label>
            <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
            <asp:Button ID="btnOK" runat="server" Text="Show" OnClick="btnOK_Click" />
        </asp:Panel>
        <asp:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        </asp:ReportViewer>
    </div>
    </form>
</body>
</html>
