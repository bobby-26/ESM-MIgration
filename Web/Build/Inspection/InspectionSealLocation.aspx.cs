using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionSealLocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblParentLocationID.Visible = false;
        lblLocationID.Visible = false;
        lblSelectedNode.Visible = false;
        ucConfirmComplete.Visible = false;

                  
            ViewState["PAGENUMBER"] = 1;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Delete All", "DELETEALL", ToolBarDirection.Right);
            toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
           
           
           
            MenuInspectionLocation.AccessRights = this.ViewState;
            MenuInspectionLocation.MenuList = toolbar.Show();

            PhoenixToolbar toolbarexport = new PhoenixToolbar();
            toolbarexport.AddFontAwesomeButton("../Inspection/InspectionSealLocation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbarexport.AddImageLink("javascript:CallPrint('gvLocation')", "Print", "icon_print.png", "PRINT");
            toolbarexport.AddFontAwesomeButton("javascript:openNewWindow('Copy','','" + Session["sitepath"] + "/Inspection/InspectionSealLocationCopy.aspx',null);return true;", "Copy", "<i class=\"fas fa-copy\"></i>", "Copy");

            MenuExport.AccessRights = this.ViewState;
            MenuExport.MenuList = toolbarexport.Show();
         if (!IsPostBack)
        { 
            lblParentLocationID.Text = "Root";
            lblSelectedNode.Text = "Root";
            BindData();
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTYN" };
        string[] alCaptions = { "Location Name", "Seal Point Y/N" };

        DataSet ds = new DataSet();
        ds = PhoenixInspectionSealLocation.SealLocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);            
        General.SetPrintOptions("gvLocation", "Seal Location", alCaptions, alColumns, ds);
        tvwLocation.DataTextField = "fldlocationname";
        tvwLocation.DataValueField = "fldlocationid";
        tvwLocation.DataFieldParentID = "fldparentlocationid";
       // tvwLocation.XPathField = "xpath";
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

    protected void MenuInspectionLocation_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (lblSelectedNode.Text.ToLower().Equals("_root"))
                lblSelectedNode.Text = "";
            // string parentid = tvwLocation.SelectedNode != null ? tvwLocation.SelectedNode.Value : "";
            if (lblLocationID.Text == "")
            {
                if (txtLocationName.Text == "")
                {
                    ucError.ErrorMessage = "Location Name is Required";
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionSealLocation.SealLocationInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableString(lblSelectedNode.Text), General.GetNullableString(txtLocationName.Text), null, int.Parse(chkSealPoint.Checked.Value ? "1" : "0")
                    , General.GetNullableString(txtLocationNo.Text));
                ucStatus.Text = "Seal location /point added successfully.";
                BindData();

                txtLocationName.Text = "";
                txtLocationNo.Text = "";
                //txtLocationCode.Text = "";
                
            }
            else
            {
                if (txtLocationName.Text == "")
                {
                    ucError.ErrorMessage = "Location Name is Required";
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionSealLocation.SealLocationUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    lblLocationID.Text, lblSelectedNode.Text, txtLocationName.Text, null, int.Parse(chkSealPoint.Checked.Value ? "1" : "0")
                    , General.GetNullableString(txtLocationNo.Text));
                ucStatus.Text = "Seal location /point updated successfully.";
                BindData();
                //txtLocationName.Text = "";
                //txtLocationCode.Text = "";                
            }
        }
        if (CommandName.ToUpper().Equals("DELETE"))
        {
            if (txtLocationName.Text == "")
            {
                ucError.ErrorMessage = "Select Root Node to be deleted";
                ucError.Visible = true;
            }
            else
            {
                try
                {
                    PhoenixInspectionSealLocation.SealLocationDelete(lblSelectedNode.Text, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ucStatus.Text = "Seal location /point deleted successfully.";
                    BindData();
                    txtLocationName.Text = "";
                    txtLocationNo.Text = "";
                    chkSealPoint.Checked = false;
                    //txtLocationCode.Text = "";                    
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
        if (CommandName.ToUpper().Equals("DELETEALL"))
        {
            try
            {
                ucConfirmComplete.Visible = true;
                ucConfirmComplete.Text = "Do you want to delete all the seal locations / points.?";
                return;
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void Reset()
    {
        txtLocationName.Enabled = true;
        //txtLocationCode.Enabled = true;
        txtLocationName.Text = "";
        //txtLocationCode.Text = "";
        lblLocationID.Text = "";
        chkSealPoint.Checked = false;
        txtLocationNo.Text = "";
    }

    //protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    //{

    //    TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
    //    lblSelectedNode.Visible = false;
    //    lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
    //    lblLocationID.Text = tvsne.SelectedNode.Value.ToString();
    //    lblParentLocationID.Text = tvsne.SelectedNode.Value.ToString();
    //    if (lblSelectedNode.Text == "Root")
    //    {
    //        txtLocationName.Text = "";
    //        txtLocationNo.Text = "";
    //        //txtLocationCode.Text = "";
    //        lblLocationID.Text = "";
    //    }
    //    else
    //    {
    //        lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
    //        txtLocationName.Text = tvsne.SelectedNode.Text.ToString();
    //        locationcode();
    //    }
    //}

    //protected void locationcode()
    //{
    //    if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
    //    {
    //        DataSet ds = PhoenixInspectionSealLocation.SealLocationEdit(int.Parse(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            DataRow dr = ds.Tables[0].Rows[0];
    //            //txtLocationCode.Text = dr["FLDLOCATIONCODE"].ToString();
    //            lblParentLocationID.Text = dr["FLDPARENTLOCATIONID"].ToString();
    //            txtLocationName.Text = dr["FLDLOCATIONNAME"].ToString();
    //            txtLocationNo.Text = dr["FLDLOCATIONNUMBER"].ToString();
    //            chkSealPoint.Checked = (dr["FLDSEALPOINTYN"].ToString().Equals("1") ? true : false);
    //        }
    //    }
    //    else
    //    {
    //        txtLocationName.Text = "";
    //        txtLocationNo.Text = "";
    //        lblParentLocationID.Text = "";            
    //    }
    //}

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTYN" };
        string[] alCaptions = { "Location Name", "Seal Point Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());        
        DataTable dt = new DataTable();
        dt = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ds.Tables.Add(dt.Copy());
        Response.AddHeader("Content-Disposition", "attachment; filename=SealLocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Seal Location</h3></td>");
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

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixInspectionSealLocation.SealLocationDeleteAll(PhoenixSecurityContext.CurrentSecurityContext.VesselID);                
                BindData();
                txtLocationName.Text = "";
                txtLocationNo.Text = "";
                chkSealPoint.Checked = false;
                lblLocationID.Text = "";                
                lblParentLocationID.Text = "Root";
                lblSelectedNode.Text = "Root";
                ucStatus.Text = "All seal locations / points deleted successfully.";                 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void tvwLocation_NodeClickEvent(object sender, EventArgs args)
    {
        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
        lblSelectedNode.Visible = false;
           lblSelectedNode.Text = e.Node.Value.ToString();
           lblLocationID.Text = e.Node.Value.ToString();
           lblParentLocationID.Text = e.Node.Value.ToString();
        if (e.Node.Value.ToLower() != "_root")
        {
            locationcode(e.Node.Value);
        }
        else
        {
            Reset();
        }
    }
    protected void locationcode(string locationid)
    {
        try
        {

            DataSet ds = PhoenixInspectionSealLocation.SealLocationEdit(int.Parse(locationid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //txtLocationCode.Text = dr["FLDLOCATIONCODE"].ToString();

                lblParentLocationID.Text = dr["FLDPARENTLOCATIONID"].ToString();
                txtLocationName.Text = dr["FLDLOCATIONNAME"].ToString();
                txtLocationNo.Text = dr["FLDLOCATIONNUMBER"].ToString();
                chkSealPoint.Checked = (dr["FLDSEALPOINTYN"].ToString().Equals("1") ? true : false);
            }

            else
            {
                txtLocationName.Text = "";
                txtLocationNo.Text = "";
                lblParentLocationID.Text = "";
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

}
