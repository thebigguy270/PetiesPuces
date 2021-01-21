<%@ Page Title="Courriel" Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="Courriel.aspx.cs" Inherits="CourrielCode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="GestionPanier" ContentPlaceHolderID="ContenuPrincipal" runat="server">

    <style>
.orig{
   background-color: lightgray;
    font-size:15px;
    width:12%;
    height:600px;
    display:table-cell;
    border:double;
    margin-right:10px;
    font-size:small;
}
.floatLeft { float: left; }
.floatRight { float: right; }

.btnRangee{
    visibility:hidden
}
.rangeeNonLue{
    background-color:gold
}
.rangeeLue{
    background-color:white
}
.table tbody tr.surligne td{
    background-color:#3b3b3b;
    color:white
}

.modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=90);
        opacity: 0.8;
    }
.modalPopup
{
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        padding-top: 10px;
        padding-left: 10px;
        min-width: 680px;
        min-height: 400px;
}

.tdInbox {
    font-size:11px;
}



.invisible{
    visibility:hidden;
}
.cb{
    cursor: default;
}

#tbCourriels th:not(.cb) {
    cursor: pointer;
    min-width:16px;
}

</style>
<script>
    $(document).ready(function() {
    // choses jquery
        $('#tbCourriels').on('click', 'tbody tr', function (event) {
            if (!$(this).hasClass("test")) {

                $(this).addClass('surligne').siblings().removeClass('surligne');
                if ($(this).hasClass('rangeeNonLue')) {

                    $(this).removeClass('rangeeNonLue');
                    $(this).addClass('rangeeLue');
                }
                if ($("input:radio[id='Contenu_ContenuPrincipal_rblOrigine_3']").is(":checked")) {
                    $('#<%= btnModifierBrouillon.ClientID %>').removeClass('invisible');
                }
                else {
                    $('#<%= btnModifierBrouillon.ClientID %>').addClass('invisible');
                }
                $('#<%= btnRepondre.ClientID %>').removeClass('invisible');
                $('#<%= btnTransfert.ClientID %>').removeClass('invisible');
            }
        });
    $("#<%=fuAttachement.ClientID %>").on('change', function() {
        //Si le fileupload change de valeur live
        $('#<%=fichierBrouillon.ClientID %>').val("");
         $('#<%=lblFichierBrouillon.ClientID %>').text("");
        });
    //Sélection de tout
    $('#' + '<%= btnVendeurs.ClientID %>').on('click', function() {
    $("#" +'<%= lbVendeurs.ClientID %>' + ' option').prop("selected",true);
        });
    
    $('#' + '<%= btnAdmins.ClientID %>').on('click', function() {
    $("#" +'<%= lbAdmins.ClientID %>' + ' option').prop("selected",true);
    });
    $('#' + '<%= btnClients.ClientID %>').on('click', function() {
    $("#" +'<%= lbClients.ClientID %>' + ' option').prop("selected",true);
     });
       $('#cbTous').click(function(event) {   
        if(this.checked) {
            // Iterate each checkbox
            $(':checkbox').each(function () {
                if ($(this).attr('id') != 'cbTous') {
                    
                    if ( $(this).parent().parent().parent().is(":visible")) {
                        this.checked = true;
                    }
                }                     
            });
        } else {
            $(':checkbox').each(function () {
                if ($(this).attr('id') != 'cbTous') {
                    if ($(this).parent().parent().parent().is(":visible")) {
                        this.checked = false;
                    }
                }              
            });
        }
        });

        $('th').click(function () {
            if (!$(this).hasClass('cb')) {
                var table = $(this).parents('table').eq(0)
                var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
                this.asc = !this.asc
                if (!this.asc) { rows = rows.reverse() }
                for (var i = 0; i < rows.length; i++) { table.append(rows[i]) }
            }
        })
    function comparer(index) {
        return function(a, b) {
            var valA = getCellValue(a, index), valB = getCellValue(b, index)
            return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.toString().localeCompare(valB)
        }
    }
    function getCellValue(row, index){ return $(row).children('td').eq(index).text() }    

//Pagination
$('#tbCourriels').after('<div id="nav"></div>');
    var rowsShown = 8;
    var rowsTotal = $('#tbCourriels tbody tr').length;
    var numPages = rowsTotal / rowsShown;
    $('#nav').append('<a href="#" rel="' + 0 + '"><<</a> ');

    for(i = 0;i < numPages;i++) {
        var pageNum = i + 1;
        $('#nav').append('<a href="#" rel="'+i+'">'+pageNum+'</a> ');
    }
    $('#nav').append('<a href="#" rel="' + (Math.ceil(numPages)-1) + '">>></a> ');
    $('#tbCourriels tbody tr').hide();
    $('#tbCourriels tbody tr').slice(0, rowsShown).show();
    $('#nav a:first').addClass('active');
    $('#nav a').bind('click', function(){

        $('#nav a').removeClass('active');
        $(this).addClass('active');
        var currPage = $(this).attr('rel');
        var startItem = currPage * rowsShown;
        var endItem = startItem + rowsShown;
        $('#tbCourriels tbody tr').css('opacity','0.0').hide().slice(startItem, endItem).
        css('display','table-row').animate({opacity:1}, 300);
        $(':checkbox').each(function() {
                this.checked = false;                       
            });
    });





    });


        

</script>



<asp:panel runat="server" CssClass="container mt-3">
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.css">
  <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.js"></script>
    
<asp:panel ID="pnInactiviteClient" runat="server" CssClass="card">
        <asp:Panel CssClass="card-header" runat="server">
            <asp:Label Text="Courriels de l'utilisateur" runat="server" CssClass="panel-title"/><br />
        </asp:Panel>
        <asp:panel CssClass="card-body" runat="server" style="display:table-row;padding:20px; ">
            
            <asp:Panel runat="server" cssclass="orig" style="padding:5px;" ID="pnOptions"><asp:RadioButtonList runat="server" ID="rblOrigine" AutoPostBack="True" OnSelectedIndexChanged="rblOrigine_SelectedIndexChanged">
                <asp:ListItem runat="server" Value="liRec">Réception

                </asp:ListItem>
                <asp:ListItem runat="server" Value="liEnv">Envoyés

                </asp:ListItem>
                <asp:ListItem runat="server" Value="liSup"  >Supprimés

                </asp:ListItem>
                <asp:ListItem runat="server" Value="liBrouillon">Brouillons</asp:ListItem>
                
                
                <asp:ListItem runat="server" Value="liMessage">Composer un message</asp:ListItem>
                
                </asp:RadioButtonList>
                <asp:Button ID="btnSupprimerMessage" runat="server" Text="Effacer les messages sélectionnés" CssClass="btn btn-block btn-secondary" Style="font-size:small" OnClick="btnSupprimerMessage_Click" OnClientClick="if ( !confirm('Êtes vous sûr de vouloir effacer ces messages?')) return false;" />
                <asp:Button ID="btnRestaurerMessages" runat="server" Text="Restaurer les messages sélectionnés" CssClass="btn btn-block btn-secondary" Style="font-size:small" OnClick="btnRestaurerMessages_Click" OnClientClick="if ( !confirm('Êtes vous sûr de vouloir restaurer ces messages?')) return false;" />
                <asp:Button ID="btnRepondre" runat="server" Text="Répondre au message sélectionné" CssClass="btn btn-block btn-secondary" Style="font-size:small" OnClick="Button1_Click" ClientIDMode="Static" />
                <asp:Button ID="btnTransfert" runat="server" Text="Transférer le message sélectionné" CssClass="btn btn-block btn-secondary" Style="font-size:small" OnClick="Button1_Click" ClientIDMode="Static" />
            <asp:Button ID="btnModifierBrouillon" runat="server" Text="Modifier le brouillon" CssClass="btn btn-block btn-secondary" OnClick="btnModifierBrouillon_Click" />
            </asp:Panel><asp:Panel ID="pnChoixCourriel" runat="server" style="display:table-cell; width:95%; height:600px; padding:8px" >
                <div style='overflow:scroll; width:95%;height:240px;'>
                <asp:Table runat="server" ID="tbCourriels" CssClass="table" ClientIDMode="Static" Style="table-layout:fixed">
                   
                    </asp:Table></div><br /> <asp:Panel runat="server" ID="pnContenuCourriel" Style="position:fixed; overflow:auto" Height="364px" Width="778">
                        <asp:HiddenField runat="server" ID="postC" />
                        <asp:HiddenField runat="server" ID="postV" />
                        <asp:HiddenField runat="server" ID="postTous" />
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                        
                        <div style="height:120px;width:inherit;overflow:auto"><asp:HyperLink runat="server" ID="lienJoint" Target="_blank" Style="font-size:x-small;" /><br />
                        <asp:Table runat="server" ID="tbInfos"  CssClass="table" ClientIDMode="Static" style="table-layout:fixed; font-size:x-small; overflow:auto" Width="95%">
                        
                        </asp:Table></div><br /><br />
                        <asp:TextBox style="resize:none;overflow:scroll"  runat="server" ID="lblContenu" Visible="True" TextMode="MultiLine" Width="95%" Height="176px" ReadOnly="True"></asp:TextBox>
                        <br />
                        <asp:Panel ID="pnC" runat="server" ClientIDMode="Static" Style="height:100px; width:100px; overflow:hidden; display:none">
               
                        </asp:Panel>
                        </ContentTemplate>
                        
                        </asp:UpdatePanel>
                        
                        
                    </asp:Panel></asp:Panel>
            <asp:Panel runat="server" ID="pnRedactionCourriel" CssClass="col" style="display:table-cell; width:75%; min-height:600px;padding:8px">
              
                        <asp:HiddenField ID="hidDestinataires" runat="server" />
                    
                    <asp:Table runat="server" CssClass="table">
                <asp:TableHeaderRow><asp:TableHeaderCell>Envoyer un message</asp:TableHeaderCell></asp:TableHeaderRow>
                    <asp:TableRow HorizontalAlign="Left"><asp:TableCell>Envoyer à:</asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="txtDestinataire" CssClass="form-control" Enabled="false" ></asp:TextBox></asp:TableCell><asp:TableCell><asp:Button runat="server" ID="btnAjouterDestinataire" Text="Rechercher un destinataire" CssClass="btn btn-block btn-secondary" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>Sujet:</asp:TableCell><asp:TableCell ColumnSpan="2"><asp:TextBox runat="server" ID="txtSujet" CssClass="form-control" onkeydown = "return (event.keyCode!=13);" ></asp:TextBox></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>Attachement:</asp:TableCell><asp:TableCell ><asp:FileUpload runat="server" ID="fuAttachement"/><br /><asp:Label runat="server" ID="lblFichierBrouillon"/></asp:TableCell></asp:TableRow>
                    </asp:Table>
                <br />
                <br />
                    <asp:TextBox style="resize:none;overflow:scroll"  runat="server" ID="txtCourrielAEnvoyer" TextMode="MultiLine" Height="240px" Width="824px"></asp:TextBox>
                <br />
                <br />

                <asp:Table runat="server" CssClass="table">
                    <asp:TableRow >
                    <asp:TableCell><asp:Button ID="btnSauvegarder" Text="Envoyer" CssClass="btn btn-block btn-secondary" runat="server" OnClick="btnSauvegarder_Click" /></asp:TableCell>
                    <asp:TableCell><asp:Button ID="btnBrouillon" Text="Sauvegarder comme brouillon" CssClass="btn btn-block btn-dark" runat="server" OnClick="btnBrouillon_Click" /> </asp:TableCell>
                    <asp:TableCell><asp:Button ID="btnAnnuler" Text="Annuler" CssClass="btn btn-block btn-dark " runat="server" OnClick="btnAnnuler_Click" /> </asp:TableCell>
                    </asp:TableRow>
                    </asp:Table>
                <asp:HiddenField runat="server" ID="fichierBrouillon" />

                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                    cancelcontrolid="btnQuitterPopup"
	                targetcontrolid="btnAjouterDestinataire" popupcontrolid="pnPopup" 
	                popupdraghandlecontrolid="PopupHeader" drag="true"
                    BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:panel id="pnPopup" style="display: none" CssClass="modalPopup" runat="server" >
                <asp:Table runat ="server" CssClass="table" >
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell><asp:Label ID="Label4" runat="server" Text="Destinataires pour le message"></asp:Label></asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                <asp:TableRow ID="rowClients">
                    <asp:TableCell>
                    <asp:Label ID="Label1" runat="server" Text="Clients"></asp:Label> 
                    </asp:TableCell>
                    <asp:TableCell>
                <asp:ListBox runat="server" ID="lbClients"></asp:ListBox>
                        </asp:TableCell>
                    <asp:TableCell>
                <asp:Button runat="server" ID="btnClients" Text="Sélectionner tous les clients" OnClick="btnClients_Click" CssClass="btn btn-block btn-secondary" OnClientClick="return false;"></asp:Button>
                </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="rowVendeurs">
                        <asp:TableCell>
                <asp:Label ID="Label2" runat="server" Text="Vendeurs"></asp:Label> 
                            </asp:TableCell>
                    <asp:TableCell>
                    <asp:ListBox runat="server" ID="lbVendeurs"></asp:ListBox>
                        </asp:TableCell>
                <asp:TableCell>
                <asp:Button runat="server" ID="btnVendeurs" Text="Sélectionner tous les vendeurs" CssClass="btn btn-block btn-secondary" OnClientClick="return false;" ClientIDMode="Static"></asp:Button>
                </asp:TableCell>
               </asp:TableRow>
                    <asp:TableRow ID="rowAdmins">
                        <asp:TableCell>
                        <asp:Label ID="Label3" runat="server" Text="Gestionnaires"></asp:Label> </asp:TableCell>
                <asp:TableCell><asp:ListBox runat="server" ID="lbAdmins" ></asp:ListBox></asp:TableCell>
                <asp:TableCell><asp:Button runat="server" ID="btnAdmins" Text="Sélectionner tous les gestionnaires" OnClick="btnAdmins_Click" OnClientClick="return false;" CssClass="btn btn-block btn-secondary" UseSubmitBehavior="false" ClientIDMode="Static"></asp:Button></asp:TableCell></asp:TableRow>
                </asp:Table>
                <asp:Table runat="server" CssClass="table">
                <asp:TableRow>
                    <asp:TableCell>
                    <asp:Button runat="server" ID="btnOk" Text="Ok" CssClass="btn btn-block btn-secondary"  OnClick="btnOk_Click"/></asp:TableCell>
                <asp:TableCell><asp:Button runat="server" ID="btnQuitterPopup" Text="Quitter" CssClass="btn btn-block btn-dark" /></asp:TableCell></asp:TableRow>
                </asp:Table>
                    </asp:Panel>
                

            </asp:Panel>
            

        </asp:panel>
    </asp:Panel>
    </asp:panel>
    
</asp:Content>
