using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersLocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuLocation.AccessRights = this.ViewState;
        MenuLocation.MenuList = toolbar.Show();

        PhoenixToolbar toolbarexport = new PhoenixToolbar();
        toolbarexport.AddFontAwesomeButton("../Registers/RegistersLocation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarexport.AddFontAwesomeButton("javascript:CallPrint('gvLocation')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuExport.AccessRights = this.ViewState;
        MenuExport.MenuList = toolbarexport.Show();
        if (!IsPostBack)
        {                        
            BindData();
			BindDepartment();            
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDLOCATIONCODE", "FLDLOCATIONNAME" };
        string[] alCaptions = { "Location Code", "Location Name" };

        DataSet ds = new DataSet();
        ds = PhoenixLocation.LocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        General.SetPrintOptions("gvLocation", "Location", alCaptions, alColumns, ds);       
        tvwLocation.DataTextField = "FLDLOCATIONNAME";        
        tvwLocation.DataValueField = "FLDLOCATIONID";
        tvwLocation.DataFieldParentID = "FLDPARENTLOCATIONID";
        tvwLocation.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
        tvwLocation.PopulateTree(ds.Tables[0]);
    }
    protected void MenuExport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string parentid = tvwLocation.SelectedNode != null ? tvwLocation.SelectedNode.Value : "";
            if (lblLocationID.Text == "")
            {
                if (txtLocationName.Text == "")
                {
                    ucError.ErrorMessage = "Location Name is Required";
                    ucError.Visible = true;
                    return;
                }               
                PhoenixLocation.LocationInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    parentid.ToLower().Replace("_root",""), txtLocationName.Text, txtLocationCode.Text, General.GetNullableInteger(ddlDepartment.SelectedValue));
                BindData();
                tvwLocation.SelectNodeByValue = parentid;
                txtLocationName.Text = "";
                txtLocationCode.Text = "";
            }
            else
            {
                if (txtLocationName.Text == "")
                {
                    ucError.ErrorMessage = "Location Name is Required";
                    ucError.Visible = true;
                    return;
                }
                PhoenixLocation.LocationUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    lblLocationID.Text, parentid.ToLower().Replace("_root", ""), txtLocationName.Text, txtLocationCode.Text, General.GetNullableInteger(ddlDepartment.SelectedValue));
                BindData();
                locationcode(lblLocationID.Text);
                tvwLocation.SelectNodeByValue = lblLocationID.Text;
            }
        }
        if (CommandName.ToUpper().Equals("DELETE"))
        {
            if (lblLocationID.Text == "")
            {
                ucError.ErrorMessage = "Select Node to be deleted";
                ucError.Visible = true;
            }
            else
            {
                try
                {                    
                    PhoenixLocation.LocationDelete(PhoenixSecurityContext.CurrentSecurityContext.VesselID, lblLocationID.Text);
                    BindData();
                    txtLocationName.Text = "";
                    txtLocationCode.Text = "";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }

    private void Reset()
    {  
        txtLocationName.Text="";
        txtLocationCode.Text = "";
        lblLocationID.Text = "";
        ddlDepartment.SelectedValue = "";
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs args)
    {
        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
        if (e.Node.Value.ToLower() != "_root")
        {
            locationcode(e.Node.Value);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "treeheight", "PaneResized();", true);
        }
        else
        {
            Reset();
        }
    }

    protected void locationcode(string locationid)
    {

        DataSet ds = PhoenixLocation.LocationCode(PhoenixSecurityContext.CurrentSecurityContext.VesselID, locationid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtLocationCode.Text = dr["FLDLOCATIONCODE"].ToString();
            lblLocationID.Text=dr["FLDLOCATIONID"].ToString();
            txtLocationName.Text = dr["FLDLOCATIONNAME"].ToString();
            ddlDepartment.SelectedValue = dr["FLDDEPARTMENTID"].ToString();
		}
       
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLOCATIONCODE", "FLDLOCATIONNAME" };
        string[] alCaptions = { "Location Code", "Location Name"};
  
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonInventory.LocationSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, sortexpression, sortdirection,
        1,
        General.ShowRecords(null),
        ref iRowCount,
        ref iTotalPageCount);	

        Response.AddHeader("Content-Disposition", "attachment; filename=Location.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Location</h3></td>");
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
	private void BindDepartment()
	{
		DataSet ds = PhoenixLocation.DepartmentList();
		ddlDepartment.DataSource = ds;
		ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
		ddlDepartment.DataValueField = "FLDDEPARTMENTID";
		ddlDepartment.DataBind();
		ddlDepartment.Items.Insert(0, new DropDownListItem("--Select--", ""));
	}    
}
