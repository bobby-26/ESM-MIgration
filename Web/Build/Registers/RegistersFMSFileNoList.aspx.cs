using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

using Telerik.Web.UI;
public partial class RegistersFMSFileNoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSFileNoList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSFileNo')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSFileNoList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','FMS','Registers/RegistersFMSFileNoAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFMSFileNoList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersFMSFileNo.AccessRights = this.ViewState;
            MenuRegistersFMSFileNo.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;                
                gvFMSFileNo.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                bindsource();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSFileNo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
            string[] alColumns = { "FLDFILENO", "FLDFILENODESCRIPTION", "FLDFMSTYPENAME" };
            string[] alCaptions = { "File No", "Description", "Type" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string fileno = (txtFileNoSearch.Text == null) ? "" : txtFileNoSearch.Text;

            ds = PhoenixRegisterFMSMail.FMSFileNoSearch(
                fileno,
                txtDescription.Text,
                txtHint.Text,
                General.GetNullableInteger(ddlsource.SelectedValue),
                sortexpression,
                sortdirection,
                gvFMSFileNo.CurrentPageIndex + 1,
                //gvFMSFileNo.PageSize,
                int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                ref iRowCount,
                ref iTotalPageCount
                );

            Response.AddHeader("Content-Disposition", "attachment; filename=FMSFileNo.xls");
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

    protected void RegistersFMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvFMSFileNo.SelectedIndexes.Clear();
                gvFMSFileNo.EditIndexes.Clear();
                gvFMSFileNo.DataSource = null;
                gvFMSFileNo.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDescription.Text = "";
                txtFileNoSearch.Text = "";
                txtHint.Text = "";               
                gvFMSFileNo.SelectedIndexes.Clear();
                gvFMSFileNo.EditIndexes.Clear();
                gvFMSFileNo.DataSource = null;
                gvFMSFileNo.Rebind();
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
        gvFMSFileNo.Rebind();
    }

    private void bindsource()
    {
        ddlsource.DataSource = PhoenixRegisterFMSMail.FMSTypeList();
        ddlsource.DataTextField = "FLDFMSTYPENAME";
        ddlsource.DataValueField = "FLDFMSTYPEID";
        ddlsource.DataBind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDFILENODESCRIPTION", "FLDFILENOHINT", "FLDACTIVEYN" };
            string[] alCaptions = { "File No", "Description", "Hint", "Active Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string fileno = (txtFileNoSearch.Text == null) ? "" : txtFileNoSearch.Text;

            DataSet ds = PhoenixRegisterFMSMail.FMSFileNoSearch(
                                                             General.GetNullableString(fileno),
                                                             General.GetNullableString(txtDescription.Text),
                                                             General.GetNullableString(txtHint.Text),
                                                             General.GetNullableInteger(ddlsource.SelectedValue),
                                                             sortexpression,
                                                             sortdirection,
                                                             gvFMSFileNo.CurrentPageIndex + 1,
                //gvFMSFileNo.PageSize,
                                                             int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                                                             ref iRowCount,
                                                             ref iTotalPageCount
                                                             );

            General.SetPrintOptions("gvFMSFileNo", "FileNo List", alCaptions, alColumns, ds);

            gvFMSFileNo.DataSource = ds;
            gvFMSFileNo.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlsource_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvFMSFileNo.SelectedIndexes.Clear();
        gvFMSFileNo.EditIndexes.Clear();
        gvFMSFileNo.DataSource = null;
        gvFMSFileNo.Rebind();
    }

    protected void gvFMSFileNo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string uid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFMSMAILFILENOID"].ToString();
                PhoenixRegisterFMSMail.DeleteFMSFileno(new Guid(uid));
                gvFMSFileNo.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSFileNo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel fileid = (RadLabel)e.Item.FindControl("lblFileNoId");
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                    cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/Registers/RegistersFMSFileNoAdd.aspx?FileNoID=" + fileid.Text + "');return true;");
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lblFileNo");
                if (lnkEdit != null)
                {
                    lnkEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                    lnkEdit.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'FMS', '" + Session["sitepath"] + "/Registers/RegistersFMSFileNoAdd.aspx?FileNoID=" + fileid.Text + "');return true;");
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
