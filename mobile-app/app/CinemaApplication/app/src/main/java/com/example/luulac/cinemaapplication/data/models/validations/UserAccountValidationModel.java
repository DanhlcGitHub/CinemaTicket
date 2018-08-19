package com.example.luulac.cinemaapplication.data.models.validations;

public class UserAccountValidationModel {

    private String error;
    private boolean isValidation;

    public String getError() {
        return error;
    }

    public void setError(String error) {
        this.error = error;
    }

    public boolean isValidation() {
        return isValidation;
    }

    public void setValidation(boolean validation) {
        isValidation = validation;
    }
}
