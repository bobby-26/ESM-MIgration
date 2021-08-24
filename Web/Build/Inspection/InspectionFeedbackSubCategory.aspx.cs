using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionFeedbackSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionFeedbackSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFeedbackSubcategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionFeedbackSubCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuFeedbackSubcategory.AccessRights = this.ViewState;
            MenuFeedbackSubcategory.MenuList = toolbar.Show();

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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDFEEDBACKSUBCATEGORYNAME", };
        string[] alCaptions = { "S.No", "Feedback Subcategory", };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixInspectionFeedbackSubCategory.InspectionFeedbackSubcategorySearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtFeedBackSubcategoryName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=Feedback Subcategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Feedback Subcategory</h3></td>");
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

    protected void FeedbackSubcategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvFeedbackSubcategory.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                gvFeedbackSubcategory.Rebind();
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDFEEDBACKSUBCATEGORYNAME" };
        string[] alCaptions = { "S.No", "Subcategory" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionFeedbackSubCategory.InspectionFeedbackSubcategorySearch(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableString(txtFeedBackSubcategoryName.Text)
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , gvFeedbackSubcategory.CurrentPageIndex + 1
                                                                                        , gvFeedbackSubcategory.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount
                                                                                      );

        General.SetPrintOptions("gvFeedbackSubcategory", "Inspection Feedback SubCategory", alCaptions, alColumns, ds);

        gvFeedbackSubcategory.DataSource = ds;
        gvFeedbackSubcategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvFeedbackSubcategory.SelectedIndexes.Clear();
        gvFeedbackSubcategory.EditIndexes.Clear();
        gvFeedbackSubcategory.DataSource = null;
        gvFeedbackSubcategory.Rebind();
    }

    protected void gvFeedbackSubcategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = "";
                UserControlMaskNumber ucserial = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd"));
                string FeedBackSubcategoryName = ((RadTextBox)e.Item.FindControl("txtFeedBackSubcategoryNameAdd")).Text;
                if (ucserial != null)
                    serialnumber = ucserial.Text;

                if (!IsValidData(FeedBackSubcategoryName))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionFeedbackSubCategory.InsertInspectionFeedbackSubCategory(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(serialnumber)
                    , FeedBackSubcategoryName
                    , null

                    );
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid subcategoryid = new Guid(item.GetDataKeyValue("FLDFEEDBACKSUBCATEGORYID").ToString());

                PhoenixInspectionFeedbackSubCategory.InspectionFeedbackSubcategoryDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                subcategoryid);
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string subcategoryname = ((RadTextBox)e.Item.FindControl("txtFeedBackSubcategoryNameEdit")).Text;


                if (!IsValidData(subcategoryname))
                {
                    ucError.Visible = true;
                    return;
                }

                GridDataItem item = e.Item as GridDataItem;
                Guid subcategoryid = new Guid(item.GetDataKeyValue("FLDFEEDBACKSUBCATEGORYID").ToString());
                PhoenixInspectionFeedbackSubCategory.InspectionFeedbackSubCategoryupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , subcategoryid
                                            , General.GetNullableInteger(serialnumber)
                                            , subcategoryname
                                            , null
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
    protected void gvFeedbackSubcategory_ItemDataBound(object sender, GridItemEventArgs e)
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

    private bool IsValidData(string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (subcategory.Equals(""))
            ucError.ErrorMessage = "Subcategory is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvFeedbackSubcategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
