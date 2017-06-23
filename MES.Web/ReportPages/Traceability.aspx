<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Traceability.aspx.cs" Inherits="Traceability" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MES</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=no" />
    <script type="text/javascript">
        var lastNode = null;
        function SetSelectedRow(rowIndex) {
            document.getElementById("txtRowIndex").value = rowIndex;
            document.getElementById("btnFilter").click();;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script src="/js/jquery.datetimepicker.js"></script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="main-content">
                    <div class="container">
                        <div class="row">
                            <div id="content" class="col-lg-12">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="page-header">
                                            <div style="float: left; width: 500px;">
                                                <ul class="breadcrumb">
                                                    <li>
                                                        <i class="fa fa-home"></i>
                                                        <a href="/Default.aspx">首页</a>
                                                    </li>
                                                    <li>分析报表</li>
                                                    <li>追溯查询</li>
                                                </ul>
                                            </div>
                                            <div style="float: right; font-size: 12px; margin-right: 45px; margin-top: 5px">
                                                <table>
                                                    <tr>
                                                        <td style="padding-right: 10px">
                                                            <uc:CurrentUser ID="CurrentUser" runat="server" />
                                                        </td>
                                                        <td>
                                                            <uc:LangSwitchMenu ID="LangSwitchMenu" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="font-size: 12px">
                                    <div class="col-md-6" style="padding-top: 10px; padding-left: 0px; width: 100%">
                                        <table style="width: 800px" border="0">
                                            <tr>
                                                <td style="text-align: right">工单号：</td>
                                                <td style="width: 300px">
                                                    <input type="text" id="txtWO_ID" runat="server" style="width: 250px" /></td>
                                                <td style="text-align: right">日期时间：</td>
                                                <td style="width: 295px">
                                                    <input id="txtStartDateTime" type="text" style="width: 140px" runat="server" />
                                                    -
                                            <input id="txtEndDateTime" type="text" style="width: 140px" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">序列号/批次号：</td>
                                                <td>
                                                    <input type="text" id="txtPrdSN" runat="server" style="width: 250px" /> . or .

                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="CKB_BackTrace" runat="server" Text="Backwards" TextAlign="Left" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:HiddenField runat="server" ID="txtRowIndex" />
                                                    <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" Style="visibility: hidden" />
                                                    <asp:Button ID="btQuery" runat="server" Text="查询" OnClick="btQuery_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">包装号：</td>
                                                <td>
                                                    <input type="text" id="txtPackNo" runat="server" style="width: 250px;" /></td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: right">&nbsp;</td>
                                            </tr>
                                        </table>


                                    </div>
                                </div>
                                <div class="row" style="font-size: 12px; padding-left: 20px; padding-top: 5px">
                                    <asp:Label ID="lblHistory" runat="server" Text=""></asp:Label>
                                    <asp:GridView ID="gvHistory" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="gvHistory_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="prdsn" HeaderText="序列号/批次号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WO_ID" HeaderText="工单">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARTNO" HeaderText="料号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QTY" HeaderText="数量">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LINENAME" HeaderText="生产线">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_OPNO" HeaderText="工序">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_STNO" HeaderText="工站">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RESULT" HeaderText="质量状态">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QCTYPE" HeaderText="质量描述">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STARTDT" HeaderText="发生时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="142px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ENDDT" HeaderText="截至时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="142px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OPERID" HeaderText="操作人">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="wipProps" HeaderText="备注">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </div>
                                <div class="row" style="font-size: 12px; padding-left: 20px; padding-top: 10px">
                                    <asp:Label ID="lblComp" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView ID="gvComp" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvComp_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="prdsn" HeaderText="序列号/批次号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WO_ID" HeaderText="工单">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARTNO" HeaderText="料号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="consumeQTY" HeaderText="数量">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LINENAME" HeaderText="生产线">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_OPNO" HeaderText="工序">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COMPPARTNO" HeaderText="子件料号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Active" HeaderText="子件状态">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COMPSN" HeaderText="子件序列号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="consumeQTY" HeaderText="子件数量">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_STNO" HeaderText="工站">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="USEDT" HeaderText="发生时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="142px" />
                                            </asp:BoundField>
                                        </Columns>

                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </div>
                                <div class="row" style="font-size: 12px; padding-left: 20px; padding-top: 10px">
                                    <asp:Label ID="lblProcData" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView ID="gvProcData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvProcData_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="prdsn" HeaderText="序列号/批次号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WO_ID" HeaderText="工单">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARTNO" HeaderText="料号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LINENAME" HeaderText="生产线">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_OPNO" HeaderText="工序">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="L_STNO" HeaderText="工站">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARAM_ID" HeaderText="参数ID">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARAM_TEXT" HeaderText="参数值">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RESULT" HeaderText="质量状态">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PrDateTime" HeaderText="发生时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="QCPLANNO" HeaderText="质量计划">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </div>
                                <div class="row" style="font-size: 12px; padding-left: 20px; padding-top: 10px">
                                    <asp:Label ID="lblContainer" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView ID="gvContainer" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="SUBCONTAINERNO" HeaderText="序列号/批次号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CONTAINERNO" HeaderText="包装号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARTNO" HeaderText="料号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="updtime" HeaderText="发生时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="142px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OPERID" HeaderText="操作人员">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </div>
                                <div class="row" style="font-size: 12px; padding-left: 20px; padding-top: 10px">
                                    <asp:Label ID="lblTicket" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    <asp:GridView ID="gvTicket" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvTicket_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="prdsn" HeaderText="序列号/批次号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EVT_ID" HeaderText="事件号">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="60px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EVT_TYPE" HeaderText="类型">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STEPNO" HeaderText="处理步骤">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="STEPINFO" HeaderText="说明">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EXECUTOR" HeaderText="责任人">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="sub_STATUS" HeaderText="状态">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="sub_UPDDATE" HeaderText="更新时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="142px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="sub_linkdoc" HeaderText="关联文档">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>

                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
            $('#txtStartDateTime').datetimepicker();
            $('#txtEndDateTime').datetimepicker();
        </script>
    </form>
</body>
</html>

