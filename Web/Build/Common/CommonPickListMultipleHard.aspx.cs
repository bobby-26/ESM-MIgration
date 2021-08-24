using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CommonPickListMultipleHard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        //PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddImageButton("../Common/CommonPicklistMultipleUser.aspx?activeyn=" + Request["activeyn"], "Find", "search.png", "FIND");
        //MenuHardGrid.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbarmain.AddButton("Done", "DONE", ToolBarDirection.Right);

        MenuHard.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvHard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["hardtype"] != null)
                ViewState["HARDTYPE"] = Request.QueryString["hardtype"].ToString();

            if (Request.QueryString["title"] != null)
                txtTitle.Text = Request.QueryString["title"].ToString();
        }
    }

    protected void Hard_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";
            NameValueCollection nvc;

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1', 'ifMoreInfo');";
            Script += "</script>" + "\n";

            if (CommandName.ToUpper().Equals("DONE"))
            {
                if (gvHard.Items.Count > 0)
                {
                    StringBuilder strHardCode = new StringBuilder();
                    StringBuilder strHardName = new StringBuilder();

                    foreach (GridDataItem gr in gvHard.Items)
                    {
                        RadCheckBox chkSelect = (RadCheckBox)gr.FindControl("chkSelect");
                        RadLabel lblHardName = (RadLabel)gr.FindControl("lblHardName");
                        RadLabel lblHardCode = (RadLabel)gr.FindControl("lblHardCode");

                        if (chkSelect.Checked.Equals(true))
                        {
                            strHardCode.Append(lblHardCode.Text);
                            strHardCode.Append(",");

                            strHardName.Append(lblHardName.Text);
                            strHardName.Append(", ");
                        }
                    }
                    if (strHardCode.Length > 1)
                    {
                        strHardCode.Remove(strHardCode.Length - 1, 1);
                    }
                    if (strHardName.Length > 1)
                    {
                        strHardName.Remove(strHardName.Length - 2, 2);
                    }

                    nvc = Filter.CurrentPickListSelection;

                    nvc.Set(nvc.GetKey(1), strHardName.ToString());

                    nvc.Set(nvc.GetKey(2), strHardCode.ToString());

                    Filter.CurrentPickListSelection = nvc;
                }
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void HardGrid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void Rebind()
    {
        gvHard.SelectedIndexes.Clear();
        gvHard.EditIndexes.Clear();
        gvHard.DataSource = null;
        gvHard.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonRegisters.HardSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            ViewState["HARDTYPE"] != null ? ViewState["HARDTYPE"].ToString() : "", 
            "", "",
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvHard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvHard.DataSource = ds;
        gvHard.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvHard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvHard_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        
    }

    protected void gvHard_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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

    protected void gvHard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHard.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
