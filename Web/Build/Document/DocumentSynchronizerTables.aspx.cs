using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Document;
using System.Data;
using Telerik.Web.UI;

public partial class Document_DocumentSynchronizerTables : PhoenixBasePage
{
    #region Page Load Event

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            BindModuleNames();
            BindModuleTables();

        }
        BindData();
    }

    #endregion

    #region Methods

    private void BindModuleNames()
    {
        DataTable dt = PhoenixDocumentSynchronizerTables.GetModuleNamesList();
        if (dt.Rows.Count > 0)
        {
            ddlModules.DataSource = dt;
            ddlModules.DataValueField = "FLDMODULENAME";
            ddlModules.DataTextField = "FLDMODULENAME";
            ddlModules.DataBind();
        }

    }

    private void BindModuleTables()
    {
        if (ddlModules.SelectedValue.Trim() != string.Empty)
        {
            DataTable dt = PhoenixDocumentSynchronizerTables.GetModuleTablesList(ddlModules.SelectedValue.Trim());
            if (dt.Rows.Count > 0)
            {
                ddlTables.DataSource = dt;
                ddlTables.DataValueField = "FLDTABLENAME";
                ddlTables.DataTextField = "FLDTABLENAME";
                ddlTables.DataBind();
                ddlTables.Items.Insert(0, new DropDownListItem("--SELECT--", ""));
            }
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixDocumentSynchronizerTables.GetVesselModuleAssignedList(ddlModules.SelectedValue.Trim(), General.GetNullableString(ddlTables.SelectedValue.Trim()));

        gvVesselList.DataSource = ds.Tables[0];
        gvVesselList.DataBind();


    }

    #endregion

    #region Click/ Command Events

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //RadCheckBox ChkSelectVessel = (RadCheckBox)sender;
                //GridDataItem gv = (GridDataItem)ChkSelectVessel.NamingContainer;
                //RadLabel lblVesselId = (RadLabel)gv.FindControl("lblVesselId");
                //PhoenixDocumentSynchronizerTables.UpdateVesselList(ddlModules.SelectedValue.Trim(), General.GetNullableString(ddlTables.SelectedValue.Trim()),
                //    General.GetNullableInteger(lblVesselId.Text.Trim()), (ChkSelectVessel.Checked == true ? 1 : 0));
                //BindData();
                //Rebind();

                int ChkSelectVessel = ((RadCheckBox)e.Item.FindControl("ChkSelectVessel")).Checked == true ? 1 : 0;
                string lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                PhoenixDocumentSynchronizerTables.UpdateVesselList(ddlModules.SelectedValue.Trim(), General.GetNullableString(ddlTables.SelectedValue.Trim()),
                    General.GetNullableInteger(lblVesselId), (ChkSelectVessel));

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    #endregion

    #region Indexchange Events

    protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindModuleTables();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlTables_SelectedIndexChanged(object sender, EventArgs e)
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

    #endregion



    protected void Rebind()
    {
        gvVesselList.SelectedIndexes.Clear();
        gvVesselList.EditIndexes.Clear();
        gvVesselList.DataSource = null;
        gvVesselList.Rebind();
    }

    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
        BindData();
    }
}
