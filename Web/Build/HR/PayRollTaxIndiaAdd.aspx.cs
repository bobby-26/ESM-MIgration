using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollTaxIndia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid Id = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = new Guid(Request.QueryString["id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            LoadYearList();
            GetEditData();
        }
    }

    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.TaxIndiaDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRevision.Text = dr["FLDREVISION"].ToString();
                ddlYear.SelectedValue = dr["FLDYEAR"].ToString();
            }
        }
    }

    private void LoadYearList()
    {
        List<DateTime> yearList = new List<DateTime>();
        for (int i = 0; i < 15; i++)
        {
            RadComboBoxItem item1 = new RadComboBoxItem();
            item1.Text = (2011 + i).ToString();
            item1.Value = new DateTime(2011, 1, 1).AddYears(i).ToString();
            ddlYear.Items.Add(item1);
        }
        ddlYear.DataBind();
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        if (Id == Guid.Empty)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        else
        {
            toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        }
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (IsValidReport())
            {
                ucError.Visible = true;
                return;
            }

            DateTime year = Convert.ToDateTime(ddlYear.SelectedItem.Value);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollIndia.TaxIndiaInsert(usercode, year, txtRevision.Text);
            }

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.TaxIndiaUpdate(usercode, Id, year, txtRevision.Text);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";

        if (string.IsNullOrWhiteSpace(ddlYear.SelectedValue))
        {
            ucError.ErrorMessage = "Year is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtRevision.Text))
        {
            ucError.ErrorMessage = "Revision is mandatory";
        }
        return ucError.IsError;
    }
}