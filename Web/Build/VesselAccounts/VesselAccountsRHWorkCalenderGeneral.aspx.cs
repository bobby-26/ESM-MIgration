using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
partial class VesselAccountsRHWorkCalenderGeneral : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
				
				ViewState["SHIPCALENDARID"] = null;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuWorkHour.AccessRights = this.ViewState;
                MenuWorkHour.MenuList = toolbar.Show();

                rbtnadvanceretard.SelectedValue = "0";
                if (Request.QueryString["shipresthourid"] != null)
                {
                    ViewState["SHIPCALENDARID"] = Request.QueryString["shipresthourid"];
                    lblNextDateCaption.Text = "Current Date";
                }
                
                BindDetails();
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (CommandName.ToUpper().Equals("SAVE"))
			{				
				int? ShipCalendarId =   null;
                string b4text   =   string.Empty ;
                string b8text   =   string.Empty ;
                string b22text  =   string.Empty;

                if (General.GetNullableString(txtNextDate.Text) == null)
                {
                    ucError.ErrorMessage = "Please add Rest Hour Start date for this Vessel, before adding Calendar.";
                    ucError.Visible = true;
                    return;
                }

                Button btn4 = (Button)(dlstTimeList.Items[3].FindControl("btnhour"));
                if(btn4!=null) b4text= btn4.Text;
                 
                Button btn8 = (Button)(dlstTimeList.Items[7].FindControl("btnhour"));
                if(btn8!=null) b8text = btn8.Text;
                 
                Button btn22 = (Button)(dlstTimeList.Items[21].FindControl("btnhour"));
                if (btn22 != null) b22text = btn22.Text;

				if (ViewState["SHIPCALENDARID"] == null)
				{
					PhoenixVesselAccountsRH.InsertRestHourShipWorkCalendar(
							PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							PhoenixSecurityContext.CurrentSecurityContext.VesselID,
							Convert.ToDateTime(txtNextDate.Text),
							int.Parse(rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue),
                            decimal.Parse(txthours.Text),
                            int.Parse(rbnhourchange.SelectedValue ==null ? "0" : rbnhourchange.SelectedValue),
                            General.GetNullableInteger(rbnhourvalue.SelectedValue),
                            int.Parse(rbtnworkplace.SelectedValue == null ? "0" : rbtnworkplace.SelectedValue),
                            decimal.Parse(b4text),
                            decimal.Parse(b8text),
                            decimal.Parse(b22text),
                            24,
                            ref  ShipCalendarId);

                    ViewState["SHIPCALENDARID"] = ShipCalendarId;
                    
				}
				else
				{

					PhoenixVesselAccountsRH.UpdateRestHourShipWorkCalendar(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							 Convert.ToInt64(ViewState["SHIPCALENDARID"]),
							PhoenixSecurityContext.CurrentSecurityContext.VesselID,
							Convert.ToDateTime(txtNextDate.Text),
							int.Parse(rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue),
                            decimal.Parse(txthours.Text),
                            int.Parse(rbnhourchange.SelectedValue == null ? "0" : rbnhourchange.SelectedValue),
                            General.GetNullableInteger(rbnhourvalue.SelectedValue),
                            int.Parse(rbtnworkplace.SelectedValue == null ? "0" : rbtnworkplace.SelectedValue),
                            decimal.Parse(b4text),
                            decimal.Parse(b8text),
                            decimal.Parse(b22text));

				}
                ucStatus.Text = "Rest Hour Information Saved Successfully";
				ucStatus.Visible = true;
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','');";
                Script += "parent.CloseCodeHelpWindow('MoreInfo');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
			else
			{
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }	
    private void BindDetails()
    {
		DataSet ds = new DataSet();
		 string action = ViewState["SHIPCALENDARID"]==null? "ADD" : "EDIT";
         if (action == "EDIT")
         {

             ds = PhoenixVesselAccountsRH.WorkCalenderCurrentWorkDayEdit(
                                                          Convert.ToInt64(ViewState["SHIPCALENDARID"])
                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                        , int.Parse(rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue)
                                                        );
             ViewState["IDLSELECTEDVALUE"] = (rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue);
         }
         else
         {
         ds = PhoenixVesselAccountsRH.WorkCalenderNextWorkDayEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                             , int.Parse(rbtnadvanceretard.SelectedValue == null ? "0" : rbtnadvanceretard.SelectedValue));
         }
		if (ds.Tables[0].Rows.Count > 0)
		{
			
			txtStartDate.Text = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
			txtPreviousDate.Text = ds.Tables[0].Rows[0]["FLDLASTREPORTINGDATE"].ToString();
			txtNextDate.Text = ds.Tables[0].Rows[0]["FLDNEXTREPORTINGDATE"].ToString();
			txthours.Text = ds.Tables[0].Rows[0]["FLDHOURS"].ToString();
            rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDADVANCERETARD"].ToString();
            if(ds.Tables[0].Rows[0]["FLDHALFHOURYN"].ToString()!="0" && ds.Tables[0].Rows[0]["FLDHALFHOURYN"].ToString()!="")
            rbnhourvalue.SelectedValue = ds.Tables[0].Rows[0]["FLDHALFHOURYN"].ToString();
            rbtnadvanceretard.SelectedValue = ds.Tables[0].Rows[0]["FLDCLOCK"].ToString();
            rbtnworkplace.SelectedValue = ds.Tables[0].Rows[0]["FLDWORKPLACE"].ToString();
		}
        BindDataList();    
    }                                            
    protected void rbtnadvanceretard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string  IDEAL = rbtnadvanceretard.SelectedValue;
        BindDetails();
        rbtnadvanceretard.SelectedValue = IDEAL;
    }
    protected void rbnhourchange_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataList();
    }
    private void BindDataList()
    {
        if (string.IsNullOrEmpty(rbnhourvalue.SelectedValue))
            rbnhourvalue.SelectedValue = "2";
       
        string[] advancewithhalf={"0.83","0.83","0.84","23.5"};
        string[] advancewithone = { "0.67", "0.67", "0.66", "23" };
        string[] advancewithtwo = { "0.33", "0.33", "0.34", "22" };
        
        string[] retardwithhalf={"1.17","1.17","1.16","24.5"};
        string[] retardwithone = { "1.33", "1.33", "1.34", "25" };
        string[] retardwithtwo = { "1.67", "1.67", "1.66", "26" };
        

        DataTable dt = new DataTable("dummy");
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Hour", typeof(string));

        for (int i = 1; i < 25; i++)
        {
            DataRow r = dt.NewRow();
            r["Text"] = "1.0";
            r["Hour"] =(i-1).ToString().PadLeft(2, '0') + "-" + i.ToString().PadLeft(2, '0');
            dt.Rows.Add(r);
            
        }
        dlstTimeList.DataSource = dt;
        dlstTimeList.DataBind();

        if (rbnhourchange.SelectedValue == "0")
        {
            setdefault();
            return;
        }
        else if (rbnhourchange.SelectedValue == "1" && rbnhourvalue.SelectedValue =="1")
        {
            SetValues(advancewithhalf);
        }
        else if (rbnhourchange.SelectedValue == "1" && rbnhourvalue.SelectedValue == "2")
        {
            SetValues(advancewithone);
        }
        else if (rbnhourchange.SelectedValue == "1" && rbnhourvalue.SelectedValue == "3")
        {
            SetValues(advancewithtwo);
        }
        else if (rbnhourchange.SelectedValue == "2" && rbnhourvalue.SelectedValue == "1" )
        {
            SetValues(retardwithhalf);
        }
        else if (rbnhourchange.SelectedValue == "2" && rbnhourvalue.SelectedValue == "2")
        {
            SetValues(retardwithone);
        }
        else if (rbnhourchange.SelectedValue == "2" && rbnhourvalue.SelectedValue == "3")
        {
            SetValues(retardwithtwo);
        }

    }
    private void setdefault()
    {
        Button btn4 = (Button)(dlstTimeList.Items[3].FindControl("btnhour"));
        Button btn8 = (Button)(dlstTimeList.Items[7].FindControl("btnhour"));
        Button btn22 = (Button)(dlstTimeList.Items[21].FindControl("btnhour"));
        if (btn4 != null)
            btn4.BackColor = System.Drawing.Color.LightGray;   
        if (btn8 != null)
            btn8.BackColor = System.Drawing.Color.LightGray;         
        if (btn22 != null)
            btn22.BackColor = System.Drawing.Color.LightGray;       

        txthours.Text = "24";
        rbnhourvalue.ClearSelection();
        rbnhourvalue.Enabled = false;
    }
    private void SetValues(string[] value)
    {
        Button btn4 = (Button)(dlstTimeList.Items[3].FindControl("btnhour"));
        if (btn4 != null)
        {
            btn4.Text = value[0].ToString();
            btn4.BackColor = System.Drawing.Color.Yellow;
        }
         
        Button btn8 = (Button)(dlstTimeList.Items[7].FindControl("btnhour"));
        if (btn8 != null)
        {
            btn8.Text = value[1].ToString();
            btn8.BackColor = System.Drawing.Color.Yellow;
        }
         
        Button btn21 = (Button)(dlstTimeList.Items[21].FindControl("btnhour"));
        if (btn21 != null)
        {
            btn21.Text = value[2].ToString();
            btn21.BackColor = System.Drawing.Color.Yellow;
        }

        txthours.Text = value[3].ToString();
        rbnhourvalue.Enabled = true;
    }
    protected void dlstTimeList_ItemDataBound(object sender, DataListItemEventArgs de)
    {
        Button btn = (Button)de.Item.FindControl("btnhour");
        if (btn != null) btn.Attributes["OnClick"] = "return false";
    }
}

