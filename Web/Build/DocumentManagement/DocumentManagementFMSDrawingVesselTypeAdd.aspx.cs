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

public partial class DocumentManagementFMSDrawingVesselTypeAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["DRAWINGID"] = "";

            if (Request.QueryString["drawingcategoryid"] != null)
            {
                ViewState["DRAWINGID"] = Request.QueryString["drawingcategoryid"].ToString();
            }
            BindMapping();
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuVesselMapping.AccessRights = this.ViewState;
        MenuVesselMapping.MenuList = toolbar.Show();
    }

    private void BindMapping()
    {
        BindVesselTypes();
        DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSVesselMapList(new Guid(ViewState["DRAWINGID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            string mapping = ds.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString();
            txtDrawingName.Text = ds.Tables[0].Rows[0]["FLDSUBCATEGORYNAME"].ToString();
            BindCheckBoxList(cblVesselType, mapping);
        }
    }
    private void BindVesselTypes()
    {
        cblVesselType.Items.Clear();
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(1);
        cblVesselType.DataSource = ds;
        cblVesselType.DataBindings.DataTextField = "FLDTYPEDESCRIPTION";
        cblVesselType.DataBindings.DataValueField = "FLDVESSELTYPEID";
        cblVesselType.DataBind();
    }

    protected void MenuVesselMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (ViewState["DRAWINGID"] != null)
                {
                    string vessellist = ReadCheckBoxList(cblVesselType);
                    DataSet ds = PhoenixDocumentManagementFMSDrawings.FMSVesselTypeMap(new Guid(ViewState["DRAWINGID"].ToString()), vessellist);
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
            foreach (ButtonListItem item in cblVesselType.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblVesselType.Items)
            {
                item.Selected = false;
            }
        }
    }

}
