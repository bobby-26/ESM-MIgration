using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class VesselAccountsRHLockUnlock :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHLockUnlock.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRHLockUnlock')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHLockUnlock.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
			toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHLockUnlock.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
			MenuRHLockUnlock.AccessRights = this.ViewState;
            MenuRHLockUnlock.MenuList = toolbar.Show();

            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("Start Date Config", "CONGIF", ToolBarDirection.Right);
            toolbartap.AddButton("Lock/UnLock", "LOCK", ToolBarDirection.Right);
            MenuConfigTabStrip.AccessRights = this.ViewState;
            MenuConfigTabStrip.MenuList = toolbartap.Show();
            MenuConfigTabStrip.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvRHLockUnlock.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ConfigTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CONGIF"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHVesselStartDateConfiguration.aspx");
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

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDLOCKEDYNNAME", "FLDALLOW" };
        string[] alCaptions = { "ID", "Vessel", "Locked (Y/N)", "Manual Sign-On (Y/N)" };
       
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		string chklock = ddlLockYn.SelectedValue;
		string chkAllowsignon = ddlManualsignon.SelectedValue;

		ds = PhoenixVesselAccountsRH.SearchRHLockUnlock(
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
			txtVesselName.Text,
			General.GetNullableInteger(chklock), General.GetNullableInteger(chkAllowsignon));

        Response.AddHeader("Content-Disposition", "attachment; filename=RHLockUnlock.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>RH Lock Unlock</h3></td>");
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

    protected void RHLockUnlock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
			if(CommandName.ToUpper().Equals("FIND"))
			{
				Rebind();
			}
			if (CommandName.ToUpper().Equals("CLEAR"))
			{
				txtVesselName.Text = "";
				ddlLockYn.SelectedIndex = 0;
				ddlManualsignon.SelectedIndex = 0;
				ViewState["PAGENUMBER"] = 1;
                Rebind();
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

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDLOCKEDYNNAME", "FLDALLOW" };
        string[] alCaptions = { "ID", "Vessel", "Locked (Y/N)", "Manual <br> Sign-On (Y/N)" };

		string chklock = ddlLockYn.SelectedValue;
		string chkAllowsignon = ddlManualsignon.SelectedValue;

		DataSet ds = PhoenixVesselAccountsRH.SearchRHLockUnlock(
            (int)ViewState["PAGENUMBER"],
            gvRHLockUnlock.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
			txtVesselName.Text,
			General.GetNullableInteger(chklock), General.GetNullableInteger(chkAllowsignon));

        General.SetPrintOptions("gvRHLockUnlock", "RH Lock Unlock", alCaptions, alColumns, ds);

        gvRHLockUnlock.DataSource = ds;
        gvRHLockUnlock.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvRHLockUnlock_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidInput(((RadLabel)e.Item.FindControl("lblVesselId")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true)? "1" : "0"))
                {
                    ucError.Visible = true;
                    return;
                }

                RadLabel lblLockUnlockId = (RadLabel)e.Item.FindControl("lblLockUnlockId");
              
                if(lblLockUnlockId != null)
                {
                    if (!string.IsNullOrEmpty(lblLockUnlockId.Text))
                    {
                        PhoenixVesselAccountsRH.RHLockUnlockUpdate(int.Parse(lblLockUnlockId.Text),
                            int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                            int.Parse(((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true)? "1" : "0"));
                    }
                    else
                    {
                        PhoenixVesselAccountsRH.RHLockUnlockInsert(int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                            int.Parse(((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true)? "1" : "0"));
                    }
                }
                Rebind();
            }
            if(e.CommandName == "Page")
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
    protected void gvRHLockUnlock_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    private bool IsValidInput(string vesselid, string lockyn)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(lockyn) == null)
            ucError.ErrorMessage = "Lock Y/N is required.";

        return (!ucError.IsError);
    }

    protected void gvRHLockUnlock_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidInput(((RadLabel)e.Item.FindControl("lblVesselId")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true)? "1" : "0"))
            {
                ucError.Visible = true;
                return;
            }

            RadLabel lblLockUnlockId = (RadLabel)e.Item.FindControl("lblLockUnlockId");

            if (lblLockUnlockId != null)
            {
                if (!string.IsNullOrEmpty(lblLockUnlockId.Text))
                {
                    PhoenixVesselAccountsRH.RHLockUnlockUpdate(int.Parse(lblLockUnlockId.Text),
                        int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true) ? "1" : "0"));
                }
                else
                {
                    PhoenixVesselAccountsRH.RHLockUnlockInsert(int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                        int.Parse(((RadCheckBox)e.Item.FindControl("chkLockYNEdit")).Checked.Equals(true) ? "1" : "0"));
                }
            }

            PhoenixVesselAccountsRH.RHAllowCrewSignOnInsert
                        (
                            int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                            int.Parse(((RadCheckBox)e.Item.FindControl("chkAllowYNEdit")).Checked.Equals(true) ? "1" : "0")
                        );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRHLockUnlock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRHLockUnlock.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRHLockUnlock.EditIndexes.Clear();
        gvRHLockUnlock.SelectedIndexes.Clear();
        gvRHLockUnlock.DataSource = null;
        gvRHLockUnlock.Rebind();
    }
}
