function IsNullOrUndefined(value, considerEmptyGuid) {
    if (considerEmptyGuid === undefined)
        considerEmptyGuid = true;
    return value === null || value === undefined || value === "" || value === "00000000-0000-0000-0000-000000000000";
}