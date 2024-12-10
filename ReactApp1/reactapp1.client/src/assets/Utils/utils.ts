import {OrderStatusEnum} from "../Models/FrontendModels.ts";
import ScriptResources from "../resources/strings.ts";

export function getOrderStatusString(status: OrderStatusEnum): string {
    const statusMap: Record<OrderStatusEnum, string> = {
        [OrderStatusEnum.None]: ScriptResources.OrderStatusEnum_None,
        [OrderStatusEnum.Open]: ScriptResources.OrderStatusEnum_Open,
        [OrderStatusEnum.Closed]: ScriptResources.OrderStatusEnum_Closed,
        [OrderStatusEnum.Completed]: ScriptResources.OrderStatusEnum_Completed,
        [OrderStatusEnum.Cancelled]: ScriptResources.OrderStatusEnum_Cancelled
    };

    return statusMap[status] ?? "Unknown";
}

export function getYesNoString(value: boolean): string {
    if (value) {
        return ScriptResources.Yes;
    } else {
        return ScriptResources.No;
    }
}
