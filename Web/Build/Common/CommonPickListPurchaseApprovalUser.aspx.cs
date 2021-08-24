using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CommonPickListPurchaseApprovalUser : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");

         if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvApprovalUser.PageSize = General.ShowRecords(null);
         }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvApprovalUser.Rebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvApprovalUser_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCommonPurchase.ApprovalUserSearch(
                       txtUserName.Text,
                       txtName.Text,
                       sortexpression, sortdirection,
                       gvApprovalUser.CurrentPageIndex+1,
                       gvApprovalUser.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);
        gvApprovalUser.DataSource = ds;
        gvApprovalUser.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvApprovalUser_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblFirstName = (RadLabel)item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)item.FindControl("lblLastName");
                nvc.Add(lblFirstName.ID, lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblDesignation = (RadLabel)item.FindControl("lblDesignation");
                nvc.Add(lblLastName.ID, lblDesignation.Text);
                RadLabel lblUserCode = (RadLabel)item.FindControl("lblUser");
                nvc.Add(lblUserCode.ID, lblUserCode.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblFirstName = (RadLabel)item.FindControl("lblFirstName");
                RadLabel lblMiddleName = (RadLabel)item.FindControl("lblMiddleName");
                RadLabel lblLastName = (RadLabel)item.FindControl("lblLastName");
                nvc.Set(nvc.GetKey(1), lblFirstName.Text + " " + lblMiddleName.Text + " " + lblLastName.Text);
                RadLabel lblUserCode = (RadLabel)item.FindControl("lblUser");
                nvc.Set(nvc.GetKey(2), lblUserCode.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        }


    }
}
