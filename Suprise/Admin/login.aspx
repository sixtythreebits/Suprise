<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Admin.login" %>

<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <title>Admin Login</title>    
    <link href="/admin/css/bootstrap.min.css" rel="stylesheet" />
      <link href="/admin/css/sb-admin-2.css" rel="stylesheet" />        
  </head>
  <body>
    <form id="form1" runat="server">
      <div class="container">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Please Sign In</h3>
                    </div>
                    <div class="panel-body">
                            <fieldset>
                                <div class="form-group">
                                    <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control" placeholder="Username" autofocus=""></asp:TextBox>                                    
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" placeholder="password"></asp:TextBox>
                                </div>                                                                
                                <asp:Button ID="SignInButton" runat="server" CssClass="btn btn-lg btn-success btn-block" Text="Log In" OnClick="SignInButton_Click" />
                                <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px" Visible="false"></asp:Label>
                            </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
     </form>
    
  </body>
</html>
