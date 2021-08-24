using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOfferLetterCheckList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersOfferLetterCheckList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('GvOfferLetter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Registers/RegistersOfferLetterCheckList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../Registers/RegistersOfferLetterCheckList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                GvOfferLetter.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                GvOfferLetter.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtShortCodeFilter.Text = "";               
                BindData();
                GvOfferLetter.Rebind();                          
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDDESCRIPTION", "FLDORDERNUMBER", "FLDSIGNATUREREQYESNO", "FLDACIVEYENNO" };
        string[] alCaptions = { "Short Code", "Description", "Order Sequence", "Signature Required", "Active" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixRegistersOfferLetterCheckList.OfferLetterSearch(General.GetNullableString(txtShortCodeFilter.Text.Trim())
                                                                           , null
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , GvOfferLetter.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Crew Event", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTCODE", "FLDDESCRIPTION", "FLDORDERNUMBER", "FLDSIGNATUREREQYESNO", "FLDACIVEYENNO" };
        string[] alCaptions = { "Short Code", "Description", "Order Sequence", "Signature Required", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersOfferLetterCheckList.OfferLetterSearch(General.GetNullableString(txtShortCodeFilter.Text.Trim())
                                                                             , null
                                                                             , sortexpression
                                                                             , sortdirection
                                                                             , (int)ViewState["PAGENUMBER"]
                                                                             , GvOfferLetter.PageSize
                                                                             , ref iRowCount
                                                                             , ref iTotalPageCount);


        General.SetPrintOptions("GvOfferLetter", "Offer Letter CheckList", alCaptions, alColumns, ds);

        GvOfferLetter.DataSource = ds;
        GvOfferLetter.VirtualItemCount = iRowCount;


    }

    protected void GvOfferLetter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text;
                string desc = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;
                string ordernumber = ((UserControlNumber)e.Item.FindControl("ucordersequenceAdd")).Text;


                if (!IsValidOfferLetter(shortcode, desc, ordernumber))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersOfferLetterCheckList.InsertOfferLetterQuestion(
                                                                     shortcode
                                                                     , desc
                                                                     , General.GetNullableInteger(ordernumber)
                                                                     , ((RadCheckBox)e.Item.FindControl("chkSignatureReqAdd")).Checked == true ? 1 : 0,
                                                                      ((RadCheckBox)e.Item.FindControl("chkactiveadd")).Checked == true ? 1 : 0
                                                               );

                BindData();
                GvOfferLetter.Rebind();
            }
            else if (e.CommandName == "Page")
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


    protected void GvOfferLetter_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string id = ((RadLabel)e.Item.FindControl("lblidedit")).Text;
            string shortcode = ((RadTextBox)e.Item.FindControl("txtShortCode")).Text;
            string desc = ((RadTextBox)e.Item.FindControl("txtDescription")).Text;
            string ordernumber = ((UserControlNumber)e.Item.FindControl("ucordersequenceEdit")).Text;


            if (!IsValidOfferLetter(shortcode, desc, ordernumber))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersOfferLetterCheckList.UpdateOfferLetterQuestion(
                                                          new Guid(id)
                                                          , shortcode
                                                          , desc, General.GetNullableInteger(ordernumber)
                                                         , ((RadCheckBox)e.Item.FindControl("chkSignatureReq")).Checked == true ? 1 : 0
                                                         , ((RadCheckBox)e.Item.FindControl("chkactive")).Checked == true ? 1 : 0
            );
            GvOfferLetter.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GvOfferLetter_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string id = ((RadLabel)e.Item.FindControl("lblid")).Text;

            PhoenixRegistersOfferLetterCheckList.DeleteOfferLetterQuestion(new Guid(id));

            GvOfferLetter.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void GvOfferLetter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvOfferLetter.CurrentPageIndex + 1;
        BindData();
    }

    private bool IsValidOfferLetter(string shortcode, string desc, string ordersequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Short Code is required.";

        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableInteger(ordersequence) == null)
            ucError.ErrorMessage = "Order Sequence is required.";

        return (!ucError.IsError);
    }


    protected void GvOfferLetter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

        }
    }


}