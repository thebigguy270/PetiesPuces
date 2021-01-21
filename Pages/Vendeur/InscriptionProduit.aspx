<%@ Page Language="C#" MasterPageFile="~/Pages/PageMaster.master" CodeFile="InscriptionProduit.aspx.cs" Inherits="Pages_Vendeur_InscriptionProduit" Debug="true" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">


    <style>
        .custom-file-input ~ .custom-file-label::after {
            content: "image";
        }

        .Space label {
            margin-left: 10px;
        }

        .RadioButtonWidth label {
            width: 40px
        }


        .lblFile {
            color: #808080;
            font-size: 16px;
            font-weight: bold;
        }

        .imgResize {
            height: 64px;
            width: 64px;
        }


        .file-upload {
            display: inline-block;
            overflow: hidden;
            text-align: center;
            vertical-align: middle;
            font-family: Arial;
            background: #808080;
            color: #fff;
            border-radius: 20px;
            -moz-border-radius: 20px;
            cursor: pointer;
            text-shadow: #000 1px 1px 2px;
            /*-webkit-border-radius: 6px; enelver ca pour les radius (bloque les autre propriétées)*/
        }

            .file-upload:hover {
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0061a7', endColorstr='#007dc1',GradientType=0);
                background-color: #4c0e0e;
            }

        /* The button size */
        .file-upload {
            height: 20px;
        }

            .file-upload, .file-upload span {
                width: 20px;
            }

                .file-upload input {
                    top: 0;
                    left: 0;
                    margin: 0;
                    font-size: 11px;
                    font-weight: bold;
                    /* Loses tab index in webkit if width is set to 0 */
                    opacity: 0;
                    filter: alpha(opacity=0);
                }

                .file-upload span {
                    top: 0;
                    left: 0;
                    display: inline-block;
                    /* Adjust button text vertical alignment */
                    padding-top: 5px;
                }
    </style>

  <script type="text/javascript">
  $(function() {
      $("input:file").change(function () {

          /** @type {string[]} */
          
          var fileName = $(this).val().split('\\');
          if (fileName.length>1) {
              var monNom = fileName[fileName.length - 1];
              $("#Contenu_ContenuPrincipal_lbl").html(monNom);
          }
          
     });
      });
</script>

</asp:Content>

<asp:Content ID="InscriptionProduit" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>
     <asp:HiddenField id="hiddenImage" runat="server" value=""/>
    <asp:Panel runat="server" CssClass="container">

        <asp:Panel ID="pnCommandesClient" runat="server" CssClass="card mt-3 mb-3">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Produit" runat="server" CssClass="card-title h4" />
            </asp:Panel>
            <asp:Panel runat="server" CssClass="row card-body">
                <asp:Panel ID="panelImage" runat="server" CssClass="col-3" Visible="false">
                </asp:Panel>
                <asp:Panel ID="panel" CssClass="col" runat="server">
            </asp:Panel>
        </asp:Panel>
      </asp:Panel>
        </asp:Panel>
</asp:Content>
