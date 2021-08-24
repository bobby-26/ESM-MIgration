using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaSubject : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            lblMainSubjectID.Visible = false;
            lblSubjectID.Visible = false;
            lblSelectedNode.Visible = false;
            ucConfirmComplete.Visible = false;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");
            toolbar.AddButton("Save", "SAVE");
            toolbar.AddButton("Delete", "DELETE");
            MenuMainSub.AccessRights = this.ViewState;
            MenuMainSub.MenuList = toolbar.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("../PreSea/PreSeaSubject.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuPreSeaSubject.AccessRights = this.ViewState;
            MenuPreSeaSubject.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                lblMainSubjectID.Text = "Root";
                lblSelectedNode.Text = "Root";
                BindData();
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
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSUBJECTNAME", "FLDSUBTYPE" };
        string[] alCaptions = { "Subject Name", "Subject Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPreSeaSubject.PreSeaSubjectTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaSubjects.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PreSea Subjects</h3></td>");
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

    protected void MenuPreSeaSubject_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;            
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuMainSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (lblSubjectID.Text == "")
            {
                if (!IsValidPreSeaSubjects(txtSubjectName.Text, ddlSubjectType.SelectedValue))
                {                   
                    ucError.Visible = true;
                    return;
                }
                try
                {
                    PhoenixPreSeaSubject.InsertPreSeaSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableString(txtSubjectName.Text)
                        , int.Parse(ddlSubjectType.SelectedValue)
                        , int.Parse("1")
                        , lblSelectedNode.Text);

                    ucStatus.Text = "Subject added successfully.";
                    BindData();
                    txtSubjectName.Text = "";
                    ddlSubjectType.SelectedValue = "Dummy";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                if (!IsValidPreSeaSubjects(txtSubjectName.Text,ddlSubjectType.SelectedValue))
                {                   
                    ucError.Visible = true;
                    return;
                }
                try
                {
                    PhoenixPreSeaSubject.UpdatePreSeaSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , lblSubjectID.Text
                            , txtSubjectName.Text
                            , General.GetNullableInteger(ddlSubjectType.SelectedValue)
                            , int.Parse("1")
                            , lblSelectedNode.Text);
                    ucStatus.Text = "Subject updated successfully.";
                    BindData();
                  
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
        }
        if (dce.CommandName.ToUpper().Equals("DELETE"))
        {
            if (txtSubjectName.Text == "")
            {
                ucError.ErrorMessage = "Select Root Node to be deleted";
                ucError.Visible = true;
            }
            else
            {
                try
                {
                    PhoenixPreSeaSubject.DeletePreseaSubject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , lblSelectedNode.Text);                    
                    ucStatus.Text = "Subject deleted successfully.";
                    BindData();
                    txtSubjectName.Text = "";
                    ddlSubjectType.SelectedValue = "Dummy";
                                     
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }

        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }        
    }    
    private void BindData()
    {
        string[] alColumns = { "FLDSUBJECTNAME"};
        string[] alCaptions = {"Subject Name"};

        DataSet ds = PhoenixPreSeaSubject.PreSeaSubjectTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        //General.SetPrintOptions("gvPreSeaSubject", "Pre-Sea Subject", alCaptions, alColumns, ds);
        tvwLocation.DataTextField = "FLDSUBJECTNAME";
        tvwLocation.DataValueField = "FLDSUBJECTID";
        tvwLocation.ParentNodeField = "FLDMAINSUBJECTID";
        tvwLocation.XPathField = "XPATH";
        tvwLocation.RootText = "Root";
        if (ds.Tables[0].Rows.Count > 0)
        {
            tvwLocation.PopulateTree(ds);
        }
        else
        {
            tvwLocation.RootText = "";
            tvwLocation.PopulateTree(ds);                        
        }
    }

    private void Reset()
    {
        txtSubjectName.Enabled = true; 
        txtSubjectName.Text = "";
        lblSubjectID.Text = "";
        ddlSubjectType.SelectedValue = "Dummy";
       
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        try
        {
            TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
            lblSelectedNode.Visible = false;
            lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
            lblSubjectID.Text = tvsne.SelectedNode.Value.ToString();
            lblMainSubjectID.Text = tvsne.SelectedNode.Value.ToString();
            if (lblSelectedNode.Text == "Root")
            {
                txtSubjectName.Text = "";                                
                lblSubjectID.Text = "";
                ddlSubjectType.SelectedValue = "Dummy";
            }
            else
            {
                lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
                txtSubjectName.Text = tvsne.SelectedNode.Text.ToString();
                subjectcode();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void subjectcode()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            DataSet ds = PhoenixPreSeaSubject.EditPreSeaSubject(int.Parse(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];                
                lblMainSubjectID.Text = dr["FLDMAINSUBJECTID"].ToString();

                if (General.GetNullableInteger(dr["FLDSUBTYPE"].ToString()) != null)
                    ddlSubjectType.SelectedValue = dr["FLDSUBTYPE"].ToString();
                
                txtSubjectName.Text = dr["FLDSUBJECTNAME"].ToString();                
               

            }
        }
        else
        {
            txtSubjectName.Text = "";            
            lblMainSubjectID.Text = "";
        }
    }
    private bool IsValidPreSeaSubjects(string SubjectName, string subjecttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(SubjectName))
        {
            ucError.ErrorMessage = "Subject Name is required.";
        }
        if (General.GetNullableInteger(subjecttype) == null)
        {
            ucError.ErrorMessage = "Subject Type is required.";
        }

        return (!ucError.IsError);
    }
    protected void btnComplete_Click(object sender, EventArgs e)
    {
        
    }  
}
