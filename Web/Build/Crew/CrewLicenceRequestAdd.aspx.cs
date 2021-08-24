using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceRequestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestAdd.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewLicenceRequestAdd.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuLicenceList.AccessRights = this.ViewState;
            MenuLicenceList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["Empid"] = null;
                ViewState["newapp"] = null;

                if (Request.QueryString["Empid"] != null && Request.QueryString["Empid"].ToString() != string.Empty)
                {
                    ViewState["Empid"] = Request.QueryString["Empid"].ToString();

                    if (Request.QueryString["newapp"] != null)
                    {
                        rblCrewFrom.SelectedValue = Request.QueryString["newapp"].ToString();
                        MenuHeader.Visible = false;                        
                    }

                }
                else
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("List", "BACK", ToolBarDirection.Right);                    
                    MenuHeader.AccessRights = this.ViewState;
                    MenuHeader.MenuList = toolbar.Show();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestList.aspx");
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {

                BindData();
                gvCrewSearch.Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDate.Text = "";
                txtEmployeeNumber.Text = "";
                txtName.Text = "";
                ddlVessel.SelectedVessel = "";
                ddlRank.SelectedRank = "";

                BindData();
                gvCrewSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string empid = (ViewState["Empid"] == null) ? null : (ViewState["Empid"].ToString());

        try
        {
            DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceEmployeeSearch(txtName.Text.Trim()
                                                                                , txtEmployeeNumber.Text.Trim()
                                                                                , General.GetNullableInteger(empid)
                                                                                , 1
                                                                                , General.GetNullableByte(rblCrewFrom.SelectedValue)
                                                                                , sortexpression, sortdirection
                                                                                 , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                 , gvCrewSearch.PageSize
                                                                                , ref iRowCount, ref iTotalPageCount);
            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.VirtualItemCount = iRowCount;
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
        gvCrewSearch.Rebind();
        
    }
    private bool IsValidLicence(string Rank, string Vessel, string date)
    {
        int resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(Vessel, out resultInt))
            ucError.ErrorMessage = "Vessel is required";

        if (!int.TryParse(Rank, out resultInt))
            ucError.ErrorMessage = "Rank is required";

        if (string.IsNullOrEmpty(txtDate.Text) && txtDate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Crew Change Date is required.";

        else if (DateTime.TryParse(txtDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Crew Change Date should be greater than current date";
        }

        return (!ucError.IsError);
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("MISSINGLICENCE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                RadLabel lblEmpId = ((RadLabel)e.Item.FindControl("lblEmployeeid"));

                string empid = lblEmpId.Text;

                if (!IsValidLicence(ddlRank.SelectedRank, ddlVessel.SelectedVessel, txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                Response.Redirect("../Crew/CrewMissingLicenceRequest.aspx?empid=" + empid + "&rnkid=" + ddlRank.SelectedRank + "&vslid=" + ddlVessel.SelectedVessel + "&jdate=" + txtDate.Text + "&from=LicenceAdd", false);
                gvCrewSearch.Rebind();
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
}