using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Accounts_Accountsuseraccountmap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
    
        txtusercodes.Attributes.Add("style", "Display:None");
        lblEmail.Attributes.Add("style", "Display:None");
        cmdHiddenPick.Attributes.Add("style", "Display:None");
        txtUserCode.Attributes.Add("style", "Display:None");

        ImgAccountsUserIdPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '"+Session["sitepath"]+"/Common/CommonPickListUser.aspx', true); ");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvUserVesselmap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
             
            ViewState["SUPPLIERACCOUNTMAPID"] = "";
     }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void UserAccessList_TabStripCommand(object sender, EventArgs e)
    {

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        gvUserVesselmap.DataSource = PhoenixAccountsUserVesselMap.PhoenixAccountsUserVesselMapsearch(sortexpression
                       , sortdirection
                       , int.Parse(ViewState["PAGENUMBER"].ToString())
                       , gvUserVesselmap.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);
     
        gvUserVesselmap.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void gvuservesselmap_Rowcommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (txtUserCode.Text != "")
                {

                    RadLabel vesselid = (RadLabel)e.Item.FindControl("lblvesselid");

                    PhoenixAccountsUserVesselMap.Uservesselmapsave(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , int.Parse(txtUserCode.Text), int.Parse(vesselid.Text));
                    BindData();
                }
             }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (txtUserCode.Text != "")
                {

                    RadLabel vesselid = (RadLabel)e.Item.FindControl("lblvesselid");

                    PhoenixAccountsUserVesselMap.Uservesselmapsave(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , int.Parse(txtUserCode.Text), int.Parse(vesselid.Text));
                    BindData();
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
     }
    protected void gvuservesselmap_deleting(object sender, GridCommandEventArgs de)
    {

    }
    
    protected void gvuserVesselmap_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {

        }
        if (e.Item is GridDataItem)
        {
            
            if (txtUserCode.Text != "")
            {
                ImageButton imgadd = (ImageButton)e.Item.FindControl("imgAdd");
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkIncludeyn");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                cb.Checked = drv["FLDUSERCODE"].ToString().Equals(txtUserCode.Text) ? true : false;
                if (cb.Checked == true)
                {
                    cb.Enabled = true;
                    imgadd.Enabled = false;
                }
                else
                {
                    cb.Enabled = false;
                    imgadd.Enabled = true;
                }
            }
        }
    }

    protected void gvUserVesselmap_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUserVesselmap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
