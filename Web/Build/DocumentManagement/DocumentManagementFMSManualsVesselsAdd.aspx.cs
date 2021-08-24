using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;

public partial class DocumentManagementFMSManualsVesselsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuVesselMapping.AccessRights = this.ViewState;
        MenuVesselMapping.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["MANUALID"] = "";

            if (Request.QueryString["manualscategoryid"] != null)
            {
                ViewState["MANUALID"] = Request.QueryString["manualscategoryid"].ToString();
            }
            
            BindMapping();
        }
    }

    protected void BindMapping()
    {
        BindVesselTypes();
        DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSManualVesselMapList(new Guid(ViewState["MANUALID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            string mapping = ds.Tables[0].Rows[0]["FLDEXCLUDEVESSELLIST"].ToString();
            txtDrawingName.Text = ds.Tables[0].Rows[0]["FLDSUBCATEGORYNAME"].ToString();
            BindCheckBoxList(cblVessel, mapping);
        }
    }
    protected void MenuVesselMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (ViewState["MANUALID"] != null)
                {
                    string vessellist = ReadCheckBoxList(cblVessel);
                    DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSManualsVesselMap(new Guid(ViewState["MANUALID"].ToString()), vessellist);
                    ucStatus.Text = "Vessels Updated";
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

        }
    }
    private void BindVesselTypes()
    {
        cblVessel.Items.Clear();
        DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSVesselTypeMapVesselList(new Guid(ViewState["MANUALID"].ToString()));
        cblVessel.DataSource = ds;
        cblVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        cblVessel.DataBindings.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();
    }


    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
    protected void SelectAll(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = false;
            }
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        string[] vessel = list.Split(',');
        foreach (string item in vessel)
        {
            foreach (ButtonListItem li in cbl.Items)
            {
                if (li.Value == item)
                {
                    li.Selected = true;
                }
            }
        }
    }
}
