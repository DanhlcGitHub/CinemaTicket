package com.example.luulac.cinemaapplication.data.models;

        import java.io.Serializable;
        import java.util.List;

public class SeatCollectionModel implements Serializable{
    private int scheduleId;
    private List<TicketModel> ticketModels;
    private boolean isSuccesBookingTicket;

    public SeatCollectionModel() {
        this.isSuccesBookingTicket = false;
    }

    public SeatCollectionModel(int scheduleId, List<TicketModel> ticketModels) {
        this.scheduleId = scheduleId;
        this.ticketModels = ticketModels;
        this.isSuccesBookingTicket = false;
    }

    public int getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(int scheduleId) {
        this.scheduleId = scheduleId;
    }

    public List<TicketModel> getTicketModels() {
        return ticketModels;
    }

    public void setTicketModels(List<TicketModel> ticketModel) {
        this.ticketModels = ticketModel;
    }

    public boolean isSuccesBookingTicket() {
        return isSuccesBookingTicket;
    }

    public void setSuccesBookingTicket(boolean succesBookingTicket) {
        isSuccesBookingTicket = succesBookingTicket;
    }
}
