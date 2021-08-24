using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
public partial class OptionsDmsUserIdentity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsDmsUserIdentity.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvUseridentity')", "Print Grid", "icon_print.png", "PRINT");
            MenuuserIdentity.AccessRights = this.ViewState;
            MenuuserIdentity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvUseridentity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["USERCODE"] = "";

                DataTable dt = PhoenixGeneralSettings.DMSUserIdentity();
                if (dt.Rows.Count > 0)
                {
                    ViewState["USERCODE"] = dt.Rows[0]["FLDUSERCODE"].ToString();
                }

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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDIDENTITYNAME", "FLDIDENTITY", "FLDVALIDTILL" };
            string[] alCaptions = { "Name", "Identity", "ValidTill" };


            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

            DataSet ds = PhoenixGeneralSettings.DMSUserIdentitySearch(
                             null
                              , General.GetNullableInteger(usercode)
                              , sortexpression, sortdirection
                              , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                              , gvUseridentity.PageSize
                              , ref iRowCount
                              , ref iTotalPageCount);

            General.ShowExcel("User Identity", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvUseridentity.SelectedIndexes.Clear();
        gvUseridentity.EditIndexes.Clear();
        gvUseridentity.DataSource = null;
        gvUseridentity.Rebind();
    }

    protected void MenuuserIdentity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

        string[] alColumns = { "FLDIDENTITYNAME", "FLDIDENTITY", "FLDVALIDTILL" };
        string[] alCaptions = { "Name", "Identity", "ValidTill" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();

        DataSet ds = PhoenixGeneralSettings.DMSUserIdentitySearch(
                         null
                         , General.GetNullableInteger(usercode)
                         , sortexpression, sortdirection
                         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                         , gvUseridentity.PageSize
                         , ref iRowCount
                         , ref iTotalPageCount);


        General.SetPrintOptions("gvUseridentity", "User Identity", alCaptions, alColumns, ds);

        gvUseridentity.DataSource = ds;
        gvUseridentity.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvUseridentity_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }


    private void InsertInspection(string name, string identity, string validtill, int usercode)
    {

        PhoenixGeneralSettings.UserIdentityInsert
                            (name, identity, General.GetNullableDateTime(validtill), usercode);
    }

    private bool IsValidUserIdentity(string name, string identity, string validtill)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Name is required.";
        if (string.IsNullOrEmpty(identity))
            ucError.ErrorMessage = "Identity is required.";
        if (General.GetNullableInteger(ViewState["USERCODE"].ToString())== null)
        {
            ucError.ErrorMessage = "User is required.";
        }


        return (!ucError.IsError);
    }

    private void DeleteUserIdentity(string name, string identity)
    {
        PhoenixGeneralSettings.UserIdentityDelete(name, identity);
    }


    protected void gvUseridentity_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidUserIdentity(
                                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                                        , ((RadTextBox)e.Item.FindControl("txtidentityAdd")).Text
                                        , ((UserControlDate)e.Item.FindControl("txtValidtillAdd")).Text
                                         ))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertInspection
                    (
                       ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                        , ((RadTextBox)e.Item.FindControl("txtidentityAdd")).Text
                        , ((UserControlDate)e.Item.FindControl("txtValidtillAdd")).Text
                        , int.Parse(ViewState["USERCODE"].ToString())
                    );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteUserIdentity(((RadLabel)e.Item.FindControl("lblName")).Text
                                    , ((RadLabel)e.Item.FindControl("lblidentity")).Text
                                    );
                Rebind();
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

    protected void gvUseridentity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }


            {
                LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null)
                    cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvUseridentity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUseridentity.CurrentPageIndex + 1;
        BindData();
    }
}