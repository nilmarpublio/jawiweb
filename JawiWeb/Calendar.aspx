<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calendar.aspx.cs" Inherits="Calendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="description" content="Malaysian Muslim Calendar Converter" />
    <meta name="Keywords" content="Malaysian Muslim Calendar Converter" />
    <title>Malaysian Muslim Calendar Converter</title>
    <link rel="Stylesheet" href="Style.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="&gt;&gt;" OnClick="Button1_Click" />
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
    <div class="legend">
        <h2>Muslim Month</h2>
        <span class="leftCol">
            <ol>
                <li>Muharram</li>
                <li>Safar</li>
                <li>Rabiulawal</li>
                <li>Rabiulakhir</li>
                <li>Jamadilawal</li>
                <li>Jamadilakhir</li>
                <li>Rejab</li>
                <li>Syaaban</li>
                <li>Ramadhan</li>
                <li>Syawal</li>
                <li>Zulkaedah</li>
                <li>Zulhijjah</li>
            </ol>
        </span>
        <span class="rightCol">
            <ol>
                <li>محرّم</li>
                <li>صفر</li>
                <li>ربيع الاول</li>
                <li>ربيع الاخير</li>
                <li>جمادالاول</li>
                <li>جمادالاخير</li>
                <li>رجب</li>
                <li>شعبان</li>
                <li>رمضان</li>
                <li>شوال</li>
                <li>ذوالقعده</li>
                <li>ذوالحجه</li>
            </ol>
        </span>
    </div>
    <div class="footer">
    <hr />
    Copyright &copy;2008 HL Granite Manufacturing
    <br />
    Any enquiry please contact <a href="mailto:yancyn@hotmail.com">yancyn</a>
    </div>
    </form>
</body>
</html>
