using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class Accounts_AccountsErmDebitNoteReferenceAttachmentverification : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsErmDebitNoteReferenceAttachmentverification.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDebitReference')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsErmDebitNoteReferenceAttachmentverification.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsErmDebitNoteReferenceAttachmentverification.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuDebitReference.AccessRights = this.ViewState;
            MenuDebitReference.MenuList = toolbar.Show();
            txtVendorId.Attributes.Add("style", "display:none");
            txtVenderName.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ImgSupplierPickList.Enabled = true;
                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=128&txtsupcode=" + txtVendorCode.Text + "', true); ");

                gvDebitReference.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDEBINOTEREFERENCEDTKEY", "FLDYEAR", "FLDMONTHNAME", "FLDPRINCIPAL"};
        string[] alCaptions = { "ID", "Year", "Month", "Principal"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceAttachmentVerificationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
              ,sortexpression, sortdirection,
              (int)ViewState["PAGENUMBER"],
              PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
              ref iRowCount,
              ref iTotalPageCount,
             General.GetNullableInteger(txtVendorId.Text),
                                                General.GetNullableInteger(ucMonth.SelectedHard),
                                                 General.GetNullableInteger(ucYear.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=StatementReference.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Attachments Verification</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }   

    protected void MenuDebitReference_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
               
                ViewState["PAGENUMBER"] = 1;
                BindData();

               
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //ucYear.SelectedText = "";
                //ucMonth.SelectedName = "";
                //txtVendorId.Text = "";
                //txtVendorCode.Text = "";
                //BindData();
                //gvDebitReference.Rebind();
                Response.Redirect("AccountsErmDebitNoteReferenceAttachmentverification.aspx");
           
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDEBINOTEREFERENCEDTKEY", "FLDYEAR", "FLDMONTHNAME", "FLDPRINCIPAL" };
        string[] alCaptions = { "ID", "Year", "Month", "Principal" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceAttachmentVerificationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
             ,  sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvDebitReference.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                 General.GetNullableInteger(txtVendorId.Text),
                                                General.GetNullableInteger(ucMonth.SelectedHard),
                                                 General.GetNullableInteger(ucYear.SelectedValue));

        General.SetPrintOptions("gvDebitReference", "Attachments Verification", alCaptions, alColumns, ds);

        gvDebitReference.DataSource = ds;
        gvDebitReference.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindData();
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }  

    private void SetRowSelection()
    {   

        for (int i = 0; i < gvDebitReference.Items.Count; i++)
        {
            if (gvDebitReference.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["DebitNoteReferenceid"].ToString()))
            {
                gvDebitReference.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbl = ((RadLabel)gvDebitReference.Items[rowindex].FindControl("lblDebitReferenceId"));
            if (lbl != null)
                ViewState["DebitNoteReferenceid"] = lbl.Text;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    
    protected void AccountSelection(object sender, EventArgs e)
    {
        BindData();
       
    }

    protected void gvDebitReference_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebitReference.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitReference_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            int iRowno;
         
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

         

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                iRowno = int.Parse(e.CommandArgument.ToString());
                BindPageURL(iRowno);
                SetRowSelection();
            }

           

            String scriptpopup = "";
        
            if (e.CommandName.ToUpper().Equals("VIEWATTACHMENT"))
            {
                RadLabel lblDebitnoteid = (RadLabel)e.Item.FindControl("lblDebitnoteid");
                RadLabel lblIsPhoenixAttachment = (RadLabel)e.Item.FindControl("lblIsPhoenixAttachment");
                RadLabel lblFilename = (RadLabel)e.Item.FindControl("lblFilename");
                RadLabel FileDTKey = (RadLabel)e.Item.FindControl("lblAtttachmentDTKey");
                scriptpopup = String.Format(
                           "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Accounts/AccountsOwnerSOAConfirmationcheck.aspx?qDebitnoteid=" + lblDebitnoteid.Text + "&IsPhoenixAttachment=" + lblIsPhoenixAttachment.Text + "&FileDTKey=" + FileDTKey.Text + "&Filename=" + lblFilename.Text + "&showmenu=0');");
            }
            if (scriptpopup != "")
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitReference_ItemDataBound1(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
    }

    protected void gvDebitReference_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
