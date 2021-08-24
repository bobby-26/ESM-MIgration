using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegisterCrewApprovalSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["categoryid"] = null;

                if (Request.QueryString["CATEGORYID"] != null)
                {
                    ViewState["categoryid"] = Request.QueryString["CATEGORYID"].ToString();
                    DataTable dt= PhoenixRegistersCrewApprovalCategory.ApprovalCategoryEdit(General.GetNullableGuid(ViewState["categoryid"].ToString()));
                    if (dt.Rows.Count > 0) { lblapprovalcatname.Text = dt.Rows[0]["FLDAPPROVALCATEGORYNAME"].ToString(); }
                }

                gvsubcategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvsubcategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalSubCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegisterCrewApprovalSubCategoryAdd.aspx?aid=" + ViewState["categoryid"].ToString() + "',false,800,500)", "Category Level", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
     

            gvsubcategoryTab.AccessRights = this.ViewState;
            gvsubcategoryTab.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvsubcategoryTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if(CommandName.ToUpper().Equals("FIND"))
            {
                //BindData();
                gvsubcategory.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCrewApprovalCategory.CrewSubCategorySearch(new Guid(ViewState["categoryid"].ToString()), txtShortCode.Text, txtCategory.Text
                    , sortexpression, sortdirection,
                    1,
                    gvsubcategory.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvsubcategory.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvsubcategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvsubcategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCrewApprovalCategory.CrewSubCategorySearch(new Guid(ViewState["categoryid"].ToString()), txtShortCode.Text, txtCategory.Text
                    , sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvsubcategory.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

        General.SetPrintOptions("gvsubcategory", "", alCaptions, alColumns, ds);

        gvsubcategory.DataSource = ds;
        gvsubcategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvsubcategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if(e.CommandName.ToUpper()== "DELETE")
            {
                RadLabel lblsubCategoryid = (RadLabel)e.Item.FindControl("lblsubCategoryid");
                PhoenixRegistersCrewApprovalCategory.ApprovalSubCategoryDelete(General.GetNullableGuid(lblsubCategoryid.Text));
               
                gvsubcategory.Rebind();
               
            }
            else if(e.CommandName.ToUpper() =="ADD")
            {
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
                chk.Visible = true;
                string RList = "";
                string RankList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }
                else
                {
                    ucError.ErrorMessage = "RankList Is Required";
                }

                RadTextBox txtShortCodeAdd = (RadTextBox)e.Item.FindControl("txtShortCodeAdd");
                RadTextBox txtCategorynameAdd = (RadTextBox)e.Item.FindControl("txtCategorynameAdd");

                PhoenixRegistersCrewApprovalCategory.CrewApprovalSubCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                   , General.GetNullableGuid(ViewState["categoryid"].ToString())
                                                                                   , txtCategorynameAdd.Text
                                                                                   , txtShortCodeAdd.Text
                                                                                   , RankList);
              
                gvsubcategory.Rebind();

            }
            else if (e.CommandName.ToUpper() == "SAVE")
            {
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                chk.Visible = true;
                string RList = "";
                string RankList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }
                else
                {
                    ucError.ErrorMessage = "RankList Is Required";
                }

                RadTextBox txtcodeEdit = (RadTextBox)e.Item.FindControl("txtcodeEdit");
                RadTextBox txtCategoryEdit = (RadTextBox)e.Item.FindControl("txtCategoryEdit");
                RadLabel lblCategoryIdEdit = (RadLabel)e.Item.FindControl("lblCategoryIdEdit");
                RadLabel lblsubcatIdEdit = (RadLabel)e.Item.FindControl("lblsubcatIdEdit");



                PhoenixRegistersCrewApprovalCategory.CrewApprovalSubCategoryUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                   ,General.GetNullableGuid(lblsubcatIdEdit.Text)
                                                                                   , General.GetNullableGuid(ViewState["categoryid"].ToString())
                                                                                   , txtCategoryEdit.Text
                                                                                   , txtcodeEdit.Text
                                                                                   , RankList);
                gvsubcategory.EditIndexes.Clear();
                gvsubcategory.Rebind();
                

            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvsubcategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridFooterItem)
        {
            CheckBoxList chkRankAdd = (CheckBoxList)e.Item.FindControl("chkRankApplicableAdd");
            if (chkRankAdd != null)
            {
                chkRankAdd.DataSource = PhoenixRegistersCrewApprovalCategory.ListRank(General.GetNullableGuid(ViewState["categoryid"].ToString()), null);
                chkRankAdd.DataTextField = "FLDRANKNAME";
                chkRankAdd.DataValueField = "FLDRANKID";
                chkRankAdd.DataBind();
                chkRankAdd.Enabled = true;
            }
           
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            CheckBoxList chkUserGroupEdit = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
            RadLabel lblCategoryIdEdit = (RadLabel)e.Item.FindControl("lblCategoryIdEdit");
            RadLabel lblsubcatIdEdit = (RadLabel)e.Item.FindControl("lblsubcatIdEdit");
            if (chkUserGroupEdit != null)
            {
                chkUserGroupEdit.DataSource = PhoenixRegistersCrewApprovalCategory.ListRank(General.GetNullableGuid(lblCategoryIdEdit.Text), General.GetNullableGuid(lblsubcatIdEdit.Text));
                chkUserGroupEdit.DataTextField = "FLDRANKNAME";
                chkUserGroupEdit.DataValueField = "FLDRANKID";
                chkUserGroupEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankApplicableEdit");
                if (chk != null)
                {
                    foreach (ListItem li in chk.Items)
                    {
                        string[] slist = drv["FLDRANKLIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
               
            }
            RadLabel lblsubCategoryid = (RadLabel)e.Item.FindControl("lblsubCategoryid");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Attributes.Add("onclick", "openNewWindow('BookMarkScript','','" + Session["sitepath"] + "/Registers/RegisterCrewApprovalSubCategoryAdd.aspx?aid=" + ViewState["categoryid"].ToString() + "&lid=" + lblsubCategoryid.Text + "',false,800,500);");
        }



    }

    protected void gvsubcategory_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace("ASC", "").Replace("DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }


}
