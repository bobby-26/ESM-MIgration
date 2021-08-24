using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersCompanyList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('dgCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Company','Registers/RegistersCompany.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCompanyList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersCompany.AccessRights = this.ViewState;
            MenuRegistersCompany.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                chkActive.Checked = true;
                dgCompany.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgCompany_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
            string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDPLACEOFINCORPORATION" };
            string[] alCaptions = { "Company", "Reg. No.", "Place of Incorporation" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string strCompanySearch = (txtSearchCompany.Text == null) ? "" : txtSearchCompany.Text;

            ds = PhoenixRegistersCompany.CompanySearch(
                strCompanySearch,
                chkActive.Checked == true ? 1 : 0,
                sortexpression,
                sortdirection,
                dgCompany.CurrentPageIndex + 1,
                //dgCompany.PageSize,
                int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                ref iRowCount,
                ref iTotalPageCount
                );

            Response.AddHeader("Content-Disposition", "attachment; filename=Company.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Company</h3></td>");
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

    protected void RegistersCompany_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                dgCompany.CurrentPageIndex = 0;
                dgCompany.SelectedIndexes.Clear();
                dgCompany.EditIndexes.Clear();
                dgCompany.DataSource = null;
                dgCompany.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSearchCompany.Text = "";
                chkActive.Checked = true;
                dgCompany.SelectedIndexes.Clear();
                dgCompany.EditIndexes.Clear();
                dgCompany.DataSource = null;
                dgCompany.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        dgCompany.Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPANYNAME", "FLDCOMPANYREGNO", "FLDPLACEOFINCORPORATION" };
            string[] alCaptions = { "Company", "Reg. No.", "Place of Incorporation" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string strCompanySearch = (txtSearchCompany.Text == null) ? "" : txtSearchCompany.Text;

            DataSet ds = PhoenixRegistersCompany.CompanySearch(
                  strCompanySearch
                , chkActive.Checked == true ? 1 : 0
                , sortexpression
                , sortdirection
                , dgCompany.CurrentPageIndex + 1
                , dgCompany.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                );

            General.SetPrintOptions("dgCompany", "Company", alCaptions, alColumns, ds);

            dgCompany.DataSource = ds;
            dgCompany.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgCompany_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                int CompanyId = int.Parse(eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDCOMPANYID"].ToString());
                PhoenixRegistersCompany.DeleteCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode, CompanyId);
                dgCompany.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgCompany_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblCompanyId");
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                    cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Company', '" + Session["sitepath"] + "/Registers/RegistersCompany.aspx?CompanyId=" + l.Text + "');return true;");
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkCompany");
                if (lnkEdit != null)
                {
                    lnkEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                    lnkEdit.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Company', '" + Session["sitepath"] + "/Registers/RegistersCompany.aspx?CompanyId=" + l.Text + "');return true;");
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                    lnkEdit.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lnkEdit.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
