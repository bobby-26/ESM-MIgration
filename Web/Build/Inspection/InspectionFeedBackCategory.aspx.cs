using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionFeedBackCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionFeedBackCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFeedBackCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionFeedBackCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionFeedBackCategory.AccessRights = this.ViewState;
            MenuInspectionFeedBackCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDFEEDBACKCODE", "FLDFEEDBACKCATEGORYNAME" };
        string[] alCaptions = { "S.No", "Short Code", "Feedback Category" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionFeedBackCategory.FeedBackCategorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableString(txtFeedBackCategoryName.Text)
                    , sortexpression
                    , sortdirection
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=FeedbackCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Feedback Category</h3></td>");
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

    protected void InspectionFeedBackCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvFeedBackCategory.CurrentPageIndex = 0;
                gvFeedBackCategory.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDFEEDBACKCODE", "FLDFEEDBACKCATEGORYNAME" };
        string[] alCaptions = { "S.No", "Short Code", "Feedback Category" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionFeedBackCategory.FeedBackCategorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableString(txtFeedBackCategoryName.Text)
                    , sortexpression
                    , sortdirection
                    , gvFeedBackCategory.CurrentPageIndex + 1
                    , gvFeedBackCategory.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    );

        General.SetPrintOptions("gvFeedBackCategory", "Feedback Category", alCaptions, alColumns, ds);

        gvFeedBackCategory.DataSource = ds;
        gvFeedBackCategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvFeedBackCategory.SelectedIndexes.Clear();
        gvFeedBackCategory.EditIndexes.Clear();
        gvFeedBackCategory.DataSource = null;
        gvFeedBackCategory.Rebind();
    }

    protected void gvFeedBackCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = "";

                UserControlMaskNumber ucserial = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd"));
                string feedbackcategoryname = ((RadTextBox)e.Item.FindControl("txtFeedbackCategoryNameAdd")).Text;
                string feedbackcode = ((RadTextBox)e.Item.FindControl("txtFeedbackShortCodeAdd")).Text;
                RadCheckBox chkActionreqNAdd = ((RadCheckBox)e.Item.FindControl("chkActionreqNAdd"));


                if (ucserial != null)
                    serialnumber = ucserial.Text;

                if (!IsValidData(feedbackcategoryname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionFeedBackCategory.InsertFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(serialnumber)
                        , feedbackcategoryname
                        , feedbackcode
                        ,(chkActionreqNAdd.Checked) == true ? 1 : 0
                        );
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid feedbackcategoryid = new Guid(item.GetDataKeyValue("FLDFEEDBACKCATEGORYID").ToString());

                // Guid feedbackcategoryid = new Guid(e.Item.ToString());

                PhoenixInspectionFeedBackCategory.DeleteFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , feedbackcategoryid
                        );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string feedbackcategoryname = ((RadTextBox)e.Item.FindControl("txtFeedbackCategoryNameEdit")).Text;
                RadCheckBox chkActionreqEdit = ((RadCheckBox)e.Item.FindControl("chkActionreqEdit"));

                if (!IsValidData(feedbackcategoryname))
                {
                    ucError.Visible = true;
                    return;
                }
                GridDataItem item = e.Item as GridDataItem;
                Guid FeedbackCategoryId = new Guid(item.GetDataKeyValue("FLDFEEDBACKCATEGORYID").ToString());

                // Guid FeedbackCategoryId = new Guid(e.Item.ToString());

                PhoenixInspectionFeedBackCategory.UpdateFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , FeedbackCategoryId
                        , General.GetNullableInteger(serialnumber)
                        , feedbackcategoryname
                        , (chkActionreqEdit.Checked) == true ? 1 : 0
                        );
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFeedBackCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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

    private bool IsValidData(string feedbackcategoryname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (feedbackcategoryname.Equals(""))
            ucError.ErrorMessage = "Feedback Category is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvFeedBackCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
