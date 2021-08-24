using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersCompanyUserList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyUserList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('dgUser')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyUserList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyUserList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuSecurityUsers.AccessRights = this.ViewState;
            MenuSecurityUsers.MenuList = toolbar.Show();
            //MenuSecurityUsers.SetTrigger(pnlUserEntry);

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuSecurityUsers2.AccessRights = this.ViewState;
            MenuSecurityUsers2.MenuList = toolbar1.Show();
            //MenuSecurityUsers.SetTrigger(pnlUserEntry);

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = Request.QueryString["qCompanyid"];
                ViewState["COMPANYNAME"] = Request.QueryString["qCompanyName"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                lblCompany.Text = ViewState["COMPANYNAME"].ToString();
                dgUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
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

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDDEPARTMENTNAME", "FLDUSERNAME", "FLDACTIVEYN", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDACCESSRIGHTSHARDNAME" };
            string[] alCaptions = { "Department", "User Name", "Active YN", "Firstname", "Lastname", "Middlename", "Access Rights" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixAccountsCompanyUserMap.CompanyUserSearch(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(ViewState["COMPANYID"].ToString())
                , txtSearch.Text
                , null
                , null
                , General.GetNullableInteger(ucDepartment.SelectedDepartment)
                , sortexpression
                , sortdirection
                , dgUser.CurrentPageIndex + 1
                , dgUser.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=CompanyUserMap.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Company User Map Register</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSecurityUsers_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                dgUser.SelectedIndexes.Clear();
                dgUser.EditIndexes.Clear();
                dgUser.DataSource = null;
                dgUser.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Registers/RegistersCompanyList.aspx");
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSearch.Text = "";
                ucDepartment.SelectedDepartment = "";
                dgUser.SelectedIndexes.Clear();
                dgUser.EditIndexes.Clear();
                dgUser.DataSource = null;
                dgUser.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDEPARTMENTNAME", "FLDUSERNAME", "FLDACTIVEYN", "FLDFIRSTNAME", "FLDLASTNAME", "FLDMIDDLENAME", "FLDACCESSRIGHTSHARDNAME" };
            string[] alCaptions = { "Department", "User Name", "Active YN", "Firstname", "Lastname", "Middlename", "Access Rights" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsCompanyUserMap.CompanyUserSearch(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(ViewState["COMPANYID"].ToString())
                , txtSearch.Text
                , null
                , null
                , General.GetNullableInteger(ucDepartment.SelectedDepartment)
                , sortexpression
                , sortdirection
                , dgUser.CurrentPageIndex + 1
                , dgUser.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("dgUser", "User List", alCaptions, alColumns, ds);

            dgUser.DataSource = ds;
            dgUser.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!(((UserControlHard)e.Item.FindControl("ddlAccessRight")).SelectedHard).ToUpper().Equals("DUMMY")
                    ||
                    !(((UserControlHard)e.Item.FindControl("ddlAccessRight")).SelectedHard).ToUpper().Equals(""))
                {
                    if (!IsValidRights((((UserControlHard)e.Item.FindControl("ddlAccessRight")).SelectedHard)))
                    {
                        e.Canceled = true;
                        ucError.Visible = true;
                        return;
                    }

                    if (((RadTextBox)e.Item.FindControl("txtMapId")).Text == string.Empty)
                    {
                        CompanyUserMapInsert(
                             ((RadLabel)e.Item.FindControl("lblUser")).Text
                            , (((UserControlHard)e.Item.FindControl("ddlAccessRight")).SelectedHard)
                        );
                    }
                    else
                    {
                        CompanyUserMapUpdate(
                                 (((RadTextBox)e.Item.FindControl("txtMapId")).Text).ToString()
                                , ((RadLabel)e.Item.FindControl("lblUser")).Text
                                , (((UserControlHard)e.Item.FindControl("ddlAccessRight")).SelectedHard)
                        );
                    }
                    dgUser.SelectedIndexes.Clear();
                    dgUser.EditIndexes.Clear();
                    dgUser.Rebind();
                }
                else
                {
                    if (((RadTextBox)e.Item.FindControl("txtMapId")).Text != string.Empty)
                    {
                        DeleteCompanyUserMap(
                          (((RadTextBox)e.Item.FindControl("txtMapId")).Text).ToString()
                        );
                    }
                }
                dgUser.SelectedIndexes.Clear();
                dgUser.EditIndexes.Clear();
                dgUser.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CompanyUserMapInsert(string strUserId, string strAccessRight)
    {
        try
        {
            if (ViewState["COMPANYID"] != null)
                PhoenixAccountsCompanyUserMap.CompanyUserMapInsert(int.Parse(ViewState["COMPANYID"].ToString()), int.Parse(strUserId), int.Parse(strAccessRight));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CompanyUserMapUpdate(string strMapId, string strUserId, string strAccessRight)
    {
        try
        {
            if (ViewState["COMPANYID"] != null)
                PhoenixAccountsCompanyUserMap.CompanyUserMapUpdate(new Guid(strMapId), int.Parse(ViewState["COMPANYID"].ToString()), int.Parse(strUserId), int.Parse(strAccessRight));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRights(string strAccessRights)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strAccessRights.Trim().ToUpper().Equals("") || strAccessRights.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Access Rights is required.";
        return (!ucError.IsError);
    }
    protected void dgUser_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlAccessRight");
                DataRowView drvHard = (DataRowView)e.Item.DataItem;
                if (ucHard != null) ucHard.SelectedHard = drvHard["FLDACCESSRIGHTSHARDCODE"].ToString();

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DeleteCompanyUserMap(string strMapCode)
    {
        try
        {
            PhoenixAccountsCompanyUserMap.CompanyUserMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strMapCode));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
