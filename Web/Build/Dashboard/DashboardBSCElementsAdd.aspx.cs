using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;

public partial class Dashboard_DashboardBSCElementsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!Page.IsPostBack)
        {
            ViewState["SPIID"] = General.GetNullableGuid(Request.QueryString["spiid"]);
            ViewState["PERSPECTIVEID"] = General.GetNullableGuid(Request.QueryString["sspid"]);
            ViewState["STRATEGYID"] = General.GetNullableGuid(Request.QueryString["strategyid"]);

            DataTable dt = PhoenixDashboardBSSP.BSSPList();
            radcbperspective.DataSource = dt;
            radcbperspective.DataValueField = "FLDBSSTRATEGICPERSPECTIVEID";
            radcbperspective.DataTextField = "FLDBSDESCRIPTION";
            radcbperspective.DataBind();
            radcbperspective.SelectedValue = ViewState["PERSPECTIVEID"].ToString();

            DataTable dt1 = PheonixDashboardSKSPI.SPIList();

            radcbtheme.DataSource = dt1;
            radcbtheme.DataValueField = "FLDSHIPPINGSPIID";
            radcbtheme.DataTextField = "FLDSPITITLE";
            radcbtheme.DataBind();
            radcbtheme.SelectedValue = ViewState["SPIID"].ToString();


            DataTable dt2 = PheonixDashboardSKKPI.KPIList(General.GetNullableGuid(ViewState["SPIID"].ToString()));
            radcobkpi.DataSource = dt2;
            radcobkpi.DataValueField = "FLDKPIID";
            radcobkpi.DataTextField = "FLDKPI";
            radcobkpi.DataBind();

            Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());
            Guid? sspid = General.GetNullableGuid(radcbperspective.SelectedValue);
            Guid? spiid = General.GetNullableGuid(radcbtheme.SelectedValue);
            DataTable dt3 = PhoenixDashboardBSSP.BSSMISearch(BSSMID, sspid, spiid);
            ViewState["EDIT"] = 0;
            if (dt3.Rows.Count > 0)
            {
                ViewState["EDIT"] = 1;
                BindCheckBoxList(radcobkpi, dt3.Rows[0]["KPILIST"].ToString());

            }
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        if (General.GetNullableInteger(ViewState["EDIT"].ToString()) == 1)
        {
            toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        }
        else
        {
            toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        }
        Tabstripaddmenu.MenuList = toolbargrid.Show();

    }

    protected void Tabstripaddmenu_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? sspid = General.GetNullableGuid(ViewState["PERSPECTIVEID"].ToString());
                Guid? strategyid = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());

                Guid? spiid = General.GetNullableGuid(radcbtheme.SelectedValue);

                string kpiids = string.Empty;
                kpiids = General.GetNullableString(GetCheckedItemsvalues(radcobkpi, kpiids));

                if (!IsValidMap(kpiids))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSSP.BSSMIInsert(rowusercode, strategyid, sspid, spiid, kpiids);


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? sspid = General.GetNullableGuid(ViewState["PERSPECTIVEID"].ToString());
                Guid? strategyid = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());

                Guid? spiid = General.GetNullableGuid(radcbtheme.SelectedValue);

                string kpiids = string.Empty;
                kpiids = General.GetNullableString(GetCheckedItemsvalues(radcobkpi, kpiids));

                if (!IsValidMap(kpiids))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSSP.BSSMIUpdate(rowusercode, strategyid, sspid, spiid, kpiids);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                 "BookMarkScript", "closeTelerikWindow('filter', 'Filter');", true);
            }

        }



        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidMap(string kpi)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (kpi == null)
        {
            ucError.ErrorMessage = "KPI .";
        }



        return (!ucError.IsError);
    }

    protected static string GetCheckedItemsvalues(RadComboBox comboBox, string checkednames)
    {

        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {


            foreach (var item in collection)
                sb.Append(item.Value + ",");


            checkednames = sb.ToString();
        }

        return checkednames;


    }

    public static void BindCheckBoxList(RadComboBox cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindItemByValue(item) != null)
                    cbl.Items.FindItemByValue(item).Checked = true;
            }
        }

    }
}