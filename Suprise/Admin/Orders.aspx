<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="Suprise.Admin.Orders" Theme="DevEx" %>
<%@ MasterType VirtualPath="~/Admin/Admin.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row margin-bottom">
    <button class="btn btn-primary create-new" type="button">Create New</button>
</div>
<div class="row grid">
    <dx:ASPxGridView ID="OrdersGrid" ClientInstanceName="OrdersGrid" runat="server" AutoGenerateColumns="False" Width="100%" DataSourceID="OrdersDataSource" KeyFieldName="ID">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ID" Caption="Order ID" Width="80px">
                <EditItemTemplate>
                    <%#Eval("ID") %>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="IsLovePack" Caption="Love Pack" Width="80px">
                <EditItemTemplate>
                    <%#Eval("IsLovePack") %>
                </EditItemTemplate>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="Price" Caption="Price" Width="80px">
                <PropertiesTextEdit DisplayFormatString="c2"></PropertiesTextEdit>
                <EditItemTemplate>
                    <%#Eval("Price","{0:c2}") %>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Recipient" Caption="Recipient" Width="150px">
                <EditItemTemplate>
                    <%#Eval("Recipient") %>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Address" Caption="Address" Width="300px">
                <EditItemTemplate>
                    <%#Eval("Address") %>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ZipCode" Caption="Zip" Width="70px">
                <EditItemTemplate>
                    <%#Eval("ZipCode") %>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="IsPaid" Caption="Paid" Width="80px">                
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="IsDelivered" Caption="Delivered" Width="80px">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="CRTime" Caption="Order Date" Width="180px">
                <PropertiesDateEdit DisplayFormatString="MMM dd, yyyy HH:mm"></PropertiesDateEdit>
                <EditItemTemplate>
                    <%#Eval("CRTime","{0:MMM dd, yyyy HH:mm}") %>
                </EditItemTemplate>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Note" Caption="Note" Width="100px">
                <DataItemTemplate><a class="function_expand" href="<%#Container.VisibleIndex %>">View Note</a></DataItemTemplate>
                <EditItemTemplate></EditItemTemplate>
            </dx:GridViewDataTextColumn>            
            <dx:GridViewCommandColumn ShowDeleteButton="true" ShowClearFilterButton="true" ShowEditButton="true" Width="100px"></dx:GridViewCommandColumn>
            <dx:GridViewDataColumn>
                <EditItemTemplate></EditItemTemplate>
            </dx:GridViewDataColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <%#Eval("Note") %>
            </DetailRow>
        </Templates>
        <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" ShowDetailButtons="true" />
    </dx:ASPxGridView>
    <asp:ObjectDataSource ID="OrdersDataSource" runat="server" DeleteMethod="TSP_Orders" SelectMethod="ListOrders" TypeName="Core.OrdersRepository" UpdateMethod="TSP_Orders">
        <DeleteParameters>
            <asp:Parameter Name="iud" Type="Byte" DefaultValue="2" />
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="IsLovePack" Type="Boolean" />
            <asp:Parameter Name="Price" Type="Decimal" />
            <asp:Parameter Name="Recipient" Type="String" />
            <asp:Parameter Name="Address" Type="String" />
            <asp:Parameter Name="ZipCode" Type="String" />
            <asp:Parameter Name="Note" Type="String" />
            <asp:Parameter Name="IsPaid" Type="Boolean" />
            <asp:Parameter Name="IsDelivered" Type="Boolean" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="iud" Type="Byte" DefaultValue="1" />
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="IsLovePack" Type="Boolean" />
            <asp:Parameter Name="Price" Type="Decimal" />
            <asp:Parameter Name="Recipient" Type="String" />
            <asp:Parameter Name="Address" Type="String" />
            <asp:Parameter Name="ZipCode" Type="String" />
            <asp:Parameter Name="Note" Type="String" />
            <asp:Parameter Name="IsPaid" Type="Boolean" />
            <asp:Parameter Name="IsDelivered" Type="Boolean" />
        </UpdateParameters>        
    </asp:ObjectDataSource>
</div>
        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
    $(function () {
        
        $(".row.grid").on("click", ".function_expand", function () {
            OrdersGrid.ExpandDetailRow($(this).attr("href"));
            return false;
        });
    });
</script>
</asp:Content>
