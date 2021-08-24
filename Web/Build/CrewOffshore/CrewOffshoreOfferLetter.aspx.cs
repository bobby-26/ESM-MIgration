using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewOffshoreOfferLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //if (Request.QueryString["redirectedfrom"] != null && Request.QueryString["redirectedfrom"].ToString() != "")
            //toolbar.AddButton("Back", "BACK");
            toolbar.AddButton("Show PDF", "SHOWPDF", ToolBarDirection.Right);
            toolbar.AddButton("Issue Offer Letter", "SAVE",ToolBarDirection.Right);
            
            //toolbar.AddButton("Approve", "APPROVE");                
            //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','../Common/CommonApproval.aspx?docid=" + ViewState["crewplanid"].ToString() + "&mod=" + PhoenixModule.OFFSHORE + "&type=" + Offshoreapprovaltype
            //    + "&crewplanid=" + ViewState["crewplanid"].ToString() + "&appoinmentletterid=" + ViewState["appointmentletterid"].ToString()+ "'); return false;", "Approve", "", "APPROVE");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["crewplanid"] = "";
                ViewState["appointmentletterid"] = "";
                ViewState["employeeid"] = "";
                ViewState["vesselid"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                if (Request.QueryString["appointmentletterid"] != null && Request.QueryString["appointmentletterid"].ToString() != "")
                    ViewState["appointmentletterid"] = Request.QueryString["appointmentletterid"].ToString();

                if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();

                DataTable dt = PhoenixCrewOffshoreOfferLetter.InsertOfferletter(new Guid(ViewState["crewplanid"].ToString()));

                if (dt.Rows.Count > 0)
                {

                    txtComitmentmade.Text = dt.Rows[0]["FLDCOMMITMENTS"].ToString();
                    lblletterid.Text = dt.Rows[0]["FLDLETTERID"].ToString();
                }
                SetEmployeePrimaryDetails();

                gvpendingcourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvOffshoreComponent.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreOfferLetter.EmployeeList(General.GetNullableGuid(ViewState["crewplanid"].ToString()));
            //DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["employeeid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDNAME"].ToString();
                txtFirstName.ToolTip = dt.Rows[0]["FLDNAME"].ToString();
                txtAppliedRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtAppliedRank.ToolTip = dt.Rows[0]["FLDRANKNAME"].ToString();
                //txtagreedRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                //txtagreedRank.ToolTip = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtvessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtWageAgreed.Text = dt.Rows[0]["FLDSALAGREEDUSD"].ToString();
                txtContractPeriod.Text = dt.Rows[0]["FLDPLUSORMINUSRANGE"].ToString();
                lblcurrency.Text = "(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewOffshoreOfferLetter.saveOfferletterCommitment(new Guid(lblletterid.Text)
                    , General.GetNullableString(txtComitmentmade.Text));
                BindData();
                ucStatus.Text = "Offer Letter Issued";
            }

            else if (CommandName.ToUpper().Equals("SHOWPDF"))
            {
                if (lblletterid.Text != "" && lblletterid.Text != null)
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFERLETTER&showmenu=0&offerletterid="
                        + lblletterid.Text, false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableDateTime(ucContractStartDate.Text)== null)
        //    ucError.ErrorMessage = "Contract commencement date is required.";

        //if (General.GetNullableInteger(txtContractPeriodDays.Text) == null)
        //    ucError.ErrorMessage = "Tenure (Days) is required. Please provide the days in Proposal Screen.";

        //if (General.GetNullableInteger(txtPlusMinusPeriod.Text) == null)
        //    ucError.ErrorMessage = "+/- period (days) is required. Please provide the +/- period in Proposal Screen.";

        //if (General.GetNullableInteger(txtDailyWages.Text) == null)
        //    ucError.ErrorMessage = "Daily Wages(USD) is required. Please provide the wages period in Proposal Screen.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                string Offshoreapprovaltype = PhoenixCommonRegisters.GetHardCode(1, 98, "OAA");
                String scriptpopup = String.Format(
                     "javascript:Openpopup('codehelp1','','../Common/CommonApproval.aspx?docid=" + ViewState["crewplanid"].ToString() + "&mod=" + PhoenixModule.OFFSHORE + "&type=" + Offshoreapprovaltype
                    + "&crewplanid=" + ViewState["crewplanid"].ToString() + "&appoinmentletterid=" + ViewState["appointmentletterid"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    /*Component grid*/
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDHOURS", "FLDREMARKS" };
        string[] alCaptions = { "Component", "Hours Per Week", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreOfferLetter.Searchofferletter(General.GetNullableGuid(ViewState["crewplanid"].ToString()),
         sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvOffshoreComponent.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreComponent", "Offer Letter", alCaptions, alColumns, ds);
        gvOffshoreComponent.DataSource = ds.Tables[0];
     


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       
    }

 
       
    private bool IsValidData(int? Course)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Course == null)
            ucError.ErrorMessage = "Course is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
      
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvOffshoreComponent.Rebind();
        
    }



    /*** new  grid****/
   
  
   
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;


            if (cb.Checked == true)
            {
                
                //gvpendingcourse.EditItems[gvRow.s.EditIndex = gvRow.DataItemIndex;
                //gvpendingcourse.SelectedIndex = gvRow.DataItemIndex;
                //ViewState["rowindex"] = gvRow.DataItemIndex;
                //ViewState["waivedyn"] = cb.Checked ? 1 : 0;
                //ViewState["edititem"] = 1;
                BindDataPendingCourse();
                gvpendingcourse.Rebind();
                //SetPageNavigator();
            }
            else
            {
                cb.Checked = false;

                GridDataItem row = ((GridDataItem)((CheckBox)sender).NamingContainer);
                int index = row.RowIndex;
                RadLabel pendingid = (RadLabel)gvpendingcourse.Items[index].FindControl("lblpendingcouseidedit1");
                int chkSelectedit;
                if (((CheckBox)gvpendingcourse.Items[index].FindControl("chkSelectedit"))!=null)
                { 
                    chkSelectedit = ((CheckBox)gvpendingcourse.Items[index].FindControl("chkSelectedit")).Checked ? 1 : 0;
                }
                else
                {
                    chkSelectedit = ((CheckBox)gvpendingcourse.Items[index].FindControl("chkSelect")).Checked ? 1 : 0;
                }
            
            if (pendingid.Text != "")
                {
                    PhoenixCrewOffshoreOfferLetter.UpdateOfferLetterCourseFlag(
                            new Guid(lblletterid.Text),
                            new Guid(pendingid.Text.Trim()),
                             chkSelectedit
                            );
                }

              
                BindDataPendingCourse();
                gvpendingcourse.Rebind();
               

            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindDataPendingCourse();
           
        }
    }

    private void BindDataPendingCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDHOURS", "FLDREMARKS" };
        string[] alCaptions = { "Component", "Hours Per Week", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreOfferLetter.Searchofferletter(General.GetNullableGuid(ViewState["crewplanid"].ToString()),
         sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER1"].ToString()),
                gvpendingcourse.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvpendingcourse", "Offer Letter", alCaptions, alColumns, ds);
        gvpendingcourse.DataSource = ds.Tables[0];
        gvpendingcourse.VirtualItemCount = iRowCount;
      

      
    }

    protected void gvOffshoreComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreComponent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvpendingcourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER1"] = ViewState["PAGENUMBER1"] != null ? ViewState["PAGENUMBER1"] : gvpendingcourse.CurrentPageIndex + 1;
            BindDataPendingCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(General.GetNullableInteger(((RadComboBox)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedValue)
                ))
                {
                    ucError.Visible = true;
                    return;
                }
              

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixCrewOffshoreOfferLetter.DeleteOfferletter(
                     new Guid(lblletterid.Text),
                     new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblPendingCourseEdit")).Text.Trim()));

                BindData();
                gvOffshoreComponent.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(General.GetNullableInteger(((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedValue)
                ))
                {
                    ucError.Visible = true;
                    return;
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

    protected void gvpendingcourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
          
           

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER1"] = null;
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                try
                {
                    string documentid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDDOCUMENTID"].ToString();
                    string ddlArrangedEdit1 = ((RadComboBox)e.Item.FindControl("ddlArrangedEdit1")).SelectedValue;
                    string txtDurationEdit1 = ((UserControlMaskNumber)e.Item.FindControl("txtDurationEdit1")).Text;
                    string ddlCostEdit1 = ((RadComboBox)e.Item.FindControl("ddlCostEdit1")).SelectedValue;
                    string ddlAirfareByEdit1 = ((RadComboBox)e.Item.FindControl("ddlAirfareByEdit1")).SelectedValue;
                    string ddlHotelByEdit1 = ((RadComboBox)e.Item.FindControl("ddlHotelByEdit1")).SelectedValue;
                    string ddlWageApplyEdit1 = ((RadComboBox)e.Item.FindControl("ddlWageApplyEdit1")).SelectedValue;
                    string txtCostEdit1 = ((UserControlMaskNumber)e.Item.FindControl("txtCostEdit1")).Text;
                    int chkSelectedit = ((CheckBox)e.Item.FindControl("chkSelectedit")).Checked ? 1 : 0;

                    PhoenixCrewOffshoreOfferLetter.SaveOfferletter(
                            General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblpendingcouseidedit1")).Text),
                            new Guid(lblletterid.Text),
                            Convert.ToInt32(documentid),
                            General.GetNullableInteger(ddlArrangedEdit1),
                            General.GetNullableInteger(txtDurationEdit1),
                            General.GetNullableInteger(ddlCostEdit1),
                            General.GetNullableInteger(ddlAirfareByEdit1),
                            General.GetNullableInteger(ddlHotelByEdit1),
                            General.GetNullableInteger(ddlWageApplyEdit1),
                            General.GetNullableDecimal(txtCostEdit1),
                           chkSelectedit
                            );



                    
                    BindDataPendingCourse();
                    gvpendingcourse.Rebind();
                    // BindData();


                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {


                PhoenixCrewOffshoreOfferLetter.DeleteOfferletter(
                     new Guid(lblletterid.Text),
                     new Guid(((RadLabel)e.Item.FindControl("lblpendingcouseidedit1")).Text.Trim()));



                BindDataPendingCourse();
                gvpendingcourse.Rebind();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOffshoreComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
      
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            RadComboBox ddlCourseEdit = (RadComboBox)e.Item.FindControl("ddlCourseEdit");
            RadComboBox ddlArrangedEdit = (RadComboBox)e.Item.FindControl("ddlArrangedEdit");
            UserControlMaskNumber txtDurationEdit = (UserControlMaskNumber)e.Item.FindControl("txtDurationEdit");
            RadComboBox ddlCostEdit = (RadComboBox)e.Item.FindControl("ddlCostEdit");
            RadComboBox ddlAirfareByEdit = (RadComboBox)e.Item.FindControl("ddlAirfareByEdit");
            RadComboBox ddlHotelByEdit = (RadComboBox)e.Item.FindControl("ddlHotelByEdit");
            RadComboBox ddlWageApplyEdit = (RadComboBox)e.Item.FindControl("ddlWageApplyEdit");
            UserControlMaskNumber txtCostEdit = (UserControlMaskNumber)e.Item.FindControl("txtCostEdit");

            if (ddlCourseEdit != null)
            {
                ddlCourseEdit.SelectedValue = drv["FLDCOURSEID"].ToString();
            }
            if (ddlArrangedEdit != null)
            {
                ddlArrangedEdit.SelectedValue = drv["FLDARRANGEDBYID"].ToString();
            }
            if (txtDurationEdit != null)
            {
                txtDurationEdit.Text = drv["FLDDURATION"].ToString();
            }
            if (ddlCostEdit != null)
            {
                ddlCostEdit.SelectedValue = drv["FLDCOSTISPAIDBYID"].ToString();
            }
            if (ddlAirfareByEdit != null)
            {
                ddlAirfareByEdit.SelectedValue = drv["FLDAIRFAREBYID"].ToString();
            }
            if (ddlHotelByEdit != null)
            {
                ddlHotelByEdit.SelectedValue = drv["FLDHOTELBYID"].ToString();
            }
            if (ddlWageApplyEdit != null)
            {
                ddlWageApplyEdit.SelectedValue = drv["FLDWAGESAPPLYYNID"].ToString();
            }
            if (txtCostEdit != null)
            {
                txtCostEdit.Text = drv["FLDESTIMATEDCOST"].ToString();
            }
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            RadComboBox gre = (RadComboBox)e.Item.FindControl("ddlCourseEdit");
            DataRowView drvGroupRank = (DataRowView)e.Item.DataItem;
            if (gre != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixCrewOffshoreOfferLetter.CourseList(new Guid(ViewState["crewplanid"].ToString()));
                gre.DataSource = ds;
                gre.DataTextField = "FLDCOURSE";
                gre.DataValueField = "FLDDOCUMENTID";
                gre.DataBind();
                gre.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                //gre.SelectedValue = drvGroupRank["FLDGROUPRANK"].ToString();
            }

        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox gra = (RadComboBox)e.Item.FindControl("ddlCourseAdd");
            if (gra != null)
            {
                DataSet ds = new DataSet();
                ds = PhoenixCrewOffshoreOfferLetter.CourseList(new Guid(ViewState["crewplanid"].ToString()));

                gra.DataSource = ds;
                gra.DataTextField = "FLDCOURSE";
                gra.DataValueField = "FLDDOCUMENTID";
                gra.DataBind();

                gra.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
        }
    }

    protected void gvpendingcourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {


            string documentid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDDOCUMENTID"].ToString();

            DataRowView drv = (DataRowView)e.Item.DataItem;
            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
            //{

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit1");
            // RadComboBox ddlCourseEdit = (RadComboBox)e.Item.FindControl("ddlCourseEdit");
            RadComboBox ddlArrangedEdit = (RadComboBox)e.Item.FindControl("ddlArrangedEdit1");
            UserControlMaskNumber txtDurationEdit = (UserControlMaskNumber)e.Item.FindControl("txtDurationEdit1");
            RadComboBox ddlCostEdit = (RadComboBox)e.Item.FindControl("ddlCostEdit1");
            RadComboBox ddlAirfareByEdit = (RadComboBox)e.Item.FindControl("ddlAirfareByEdit1");
            RadComboBox ddlHotelByEdit = (RadComboBox)e.Item.FindControl("ddlHotelByEdit1");
            RadComboBox ddlWageApplyEdit = (RadComboBox)e.Item.FindControl("ddlWageApplyEdit1");
            UserControlMaskNumber txtCostEdit = (UserControlMaskNumber)e.Item.FindControl("txtCostEdit1");
            CheckBox chkSelectedit = (CheckBox)e.Item.FindControl("chkSelectedit");
            CheckBox chkselect = (CheckBox)e.Item.FindControl("chkselect");

            if (chkselect != null)
            {
                if (drv["FLDACTIVEYN"].ToString() == "1")
                    chkselect.Checked = true;
                else
                    chkselect.Checked = false;
            }




            if (ddlArrangedEdit != null)
            {
                ddlArrangedEdit.SelectedValue = drv["FLDARRANGEDBYID"].ToString();
            }
            if (txtDurationEdit != null)
            {
                txtDurationEdit.Text = drv["FLDDURATION"].ToString();
            }
            if (ddlCostEdit != null)
            {
                ddlCostEdit.SelectedValue = drv["FLDCOSTISPAIDBYID"].ToString();
            }
            if (ddlAirfareByEdit != null)
            {
                ddlAirfareByEdit.SelectedValue = drv["FLDAIRFAREBYID"].ToString();
            }
            if (ddlHotelByEdit != null)
            {
                ddlHotelByEdit.SelectedValue = drv["FLDHOTELBYID"].ToString();
            }
            if (ddlWageApplyEdit != null)
            {
                ddlWageApplyEdit.SelectedValue = drv["FLDWAGESAPPLYYNID"].ToString();
            }
            if (txtCostEdit != null)
            {
                txtCostEdit.Text = drv["FLDESTIMATEDCOST"].ToString();
            }
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            if (chkSelectedit != null)
            {
                chkSelectedit.Checked = true;
            }
            //}
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete1");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }


        }
    }
}
