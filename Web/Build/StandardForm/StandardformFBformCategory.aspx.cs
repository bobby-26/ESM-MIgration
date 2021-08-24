using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using Telerik.Web.UI;
public partial class StandardformFBformCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblParentLocationID.Visible = false;
        lblCategoryID.Visible = false;
        lblSelectedNode.Visible = false;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        MenuSecurityLocation.AccessRights = this.ViewState;
        MenuSecurityLocation.MenuList = toolbar.Show();

        PhoenixToolbar toolbarexport = new PhoenixToolbar();
        toolbarexport.AddFontAwesomeButton("../StandardForm/StandardformFBformCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarexport.AddFontAwesomeButton("javascript:CallPrint('tvwCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuExport.AccessRights = this.ViewState;
        MenuExport.MenuList = toolbarexport.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            lblParentLocationID.Text = "Root";
            lblSelectedNode.Text = "Root";
            BindData();

		}

    }

    protected void BindData()
    {
        string[] alColumns = { "FLDFORMCATEGORYCODE", "FLDFORMCATEGORYNAME" };
        string[] alCaptions = { "Category Code", "Category Name" };

        DataSet ds = new DataSet();
		string showall = chkShowAll.Checked ? "1" : "0";
        ds = PhoenixFormBuilder.CategoryList(int.Parse(showall));
        General.SetPrintOptions("tvwCategory", "Category", alCaptions, alColumns, ds);
        tvwCategory.DataTextField = "FLDFORMCATEGORYNAME";
		tvwCategory.DataValueField = "FLDFORMCATEGORYID";
		tvwCategory.DataFieldParentID = "FLDPARENTCATEGORYID";
        //tvwCategory.XPathField = "xpath";
		tvwCategory.RootText = "Category";
		tvwCategory.PopulateTree(ds.Tables[0]);
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
    protected void SecurityLocation_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (lblCategoryID.Text == "")
                {
                    if (txtCategoryName.Text == "")
                    {
                        ucError.ErrorMessage = "Category Name is Required";
                        ucError.Visible = true;
                        return;
                    }
                    string strActive = chkActive.Checked == true ? "1" : "0";
                    PhoenixFormBuilder.CategoryInsert(
                        lblSelectedNode.Text, txtCategoryName.Text, txtCategoryCode.Text, int.Parse(strActive));
                    BindData();

                    txtCategoryName.Text = "";
                    txtCategoryCode.Text = "";
                }
                else
                {
                    if (txtCategoryName.Text == "")
                    {
                        ucError.ErrorMessage = "Category Name is Required";
                        ucError.Visible = true;
                        return;
                    }
                    string strActive = chkActive.Checked == true ? "1" : "0";
                    PhoenixFormBuilder.CategoryUpdate(
                        int.Parse(lblCategoryID.Text), txtCategoryName.Text, txtCategoryCode.Text, int.Parse(strActive));
                    BindData();
                    //txtCategoryName.Text = "";
                    //txtCategoryCode.Text = "";
                }
            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (txtCategoryName.Text == "")
                {
                    ucError.ErrorMessage = "Select Root Node to be deleted";
                    ucError.Visible = true;
                }
                else
                {
                    try
                    {
                        PhoenixFormBuilder.CategoryDelete(int.Parse(lblSelectedNode.Text));
                        BindData();
                        txtCategoryName.Text = "";
                        txtCategoryCode.Text = "";
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
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
		txtCategoryName.Enabled = true;
		txtCategoryCode.Enabled = true;
		txtCategoryName.Text="";
		txtCategoryCode.Text = "";
        lblCategory.Text = "";
		lblCategoryID.Text = "";
        chkActive.Checked = true;

	}

    protected void ucTree_SelectNodeEvent(object sender, EventArgs args)
    {

        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)args;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
		lblCategoryID.Text = tvsne.Node.Value.ToString();
        lblParentLocationID.Text = tvsne.Node.Value.ToString();
        if (lblSelectedNode.Text == "Root")
        {

			txtCategoryName.Text = "";
            txtCategoryCode.Text = "";
			lblCategoryID.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
			txtCategoryName.Text = tvsne.Node.Text.ToString();
			Categorycode();
        }      
    }

    protected void Categorycode()
    {

        DataTable dt = PhoenixFormBuilder.CategoryEdit(int.Parse(lblSelectedNode.Text));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtCategoryCode.Text = dr["FLDFORMCATEGORYCODE"].ToString();
            lblParentLocationID.Text = dr["FLDFORMCATEGORYNAME"].ToString();
			chkActive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;
		}
       
    }

    protected void ShowExcel()
    {

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMCATEGORYCODE", "FLDFORMCATEGORYNAME" };
        string[] alCaptions = { "Category Code", "Category Name"};
  
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		string showall = chkShowAll.Checked ? "1" : "0";

		ds = PhoenixFormBuilder.CategoryList(int.Parse(showall));
        Response.AddHeader("Content-Disposition", "attachment; filename=Category.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Category</h3></td>");
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


	protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
	{
		BindData();
	}

}
