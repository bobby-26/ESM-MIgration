using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CommonPickListUser : PhoenixBasePage
{
    public PhoenixModule mod;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:None");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuUser.MenuList = toolbarmain.Show();
        //MenuUser.SetTrigger(pnlUserEntry);


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["mod"] != null && Request.QueryString["mod"].ToString() != string.Empty)
                mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
            if (mod == PhoenixModule.QUALITY)
            {
                if (Request.QueryString["departmentlist"] != null && Request.QueryString["departmentlist"].ToString() != string.Empty)
                    ucDepartment.DepartmentFilter = Request.QueryString["departmentlist"].ToString();
            }
            if (Request.QueryString["Departmentid"] != null && Request.QueryString["MOC"] == "true")
            {
                ucDepartment.SelectedDepartment = Request.QueryString["Departmentid"].ToString();
                ucDepartment.DepartmentFilter = Request.QueryString["Departmentid"].ToString();
            }
            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }

            gvUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvUser.SelectedIndexes.Clear();
                gvUser.EditIndexes.Clear();
                gvUser.DataSource = null;
                gvUser.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string strdept = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        if (Request.QueryString["mod"] != null && Request.QueryString["mod"].ToString() != string.Empty)
            mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
        if (mod == PhoenixModule.QUALITY && Request.QueryString["departmentlist"] != null && Request.QueryString["departmentlist"].ToString() != string.Empty)
        {
            if (General.GetNullableString(ucDepartment.SelectedDepartment) == null)
                strdept = Request.QueryString["departmentlist"].ToString();
            else
                strdept = General.GetNullableString(ucDepartment.SelectedDepartment);
        }
        if (Request.QueryString["Departmentid"] != null && Request.QueryString["MOC"] == "true")
        {
            strdept = Request.QueryString["Departmentid"].ToString();
        }
        else
        {
            strdept = General.GetNullableString(ucDepartment.SelectedDepartment);
        }

        ds = PhoenixUser.UserSearch(
                       txtSearch.Text, null, null,
                       strdept,
                       "", txtFullName.Text, "", null,
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvUser.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);


        gvUser.DataSource = ds;
        gvUser.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
    }

    protected void gvUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();

                RadLabel lblFirstName = (RadLabel)e.Item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)e.Item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)e.Item.FindControl("lblLastName");
                nvc.Add(lblFirstName.ID, lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)e.Item.FindControl("lblDesignation");
                nvc.Add(lblLastName.ID, lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)e.Item.FindControl("lblUserCode");
                nvc.Add(lblUserCode.ID, lblUserCode.Text);
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblFirstName = (RadLabel)e.Item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)e.Item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)e.Item.FindControl("lblLastName");
                nvc.Set(nvc.GetKey(1), lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)e.Item.FindControl("lblDesignation");
                nvc.Set(nvc.GetKey(2), lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)e.Item.FindControl("lblUserCode");
                nvc.Set(nvc.GetKey(3), lblUserCode.Text);
                RadLabel lblEmail = (RadLabel)e.Item.FindControl("lblEmail");
                nvc.Set(nvc.GetKey(4), lblEmail.Text);
                
        
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }


    
    protected void gvUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUser.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();       
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

  
}
