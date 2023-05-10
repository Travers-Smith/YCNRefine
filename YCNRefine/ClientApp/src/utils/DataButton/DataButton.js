import React from "react";
import { Button, CircularProgress } from "@mui/material";

const DataButton = ({ 
    children, 
    isLoading,
    buttonVariant, 
    onClick, 
    onMouseUp,
    onMouseDown,
    disabled,
    style,
    className,
    block,
    size
 }) => (
        <>
            <Button
                block={block && block}
                size={size && size}
                className={className}
                style={style}
                variant={buttonVariant ? buttonVariant : "text"}
                disabled={disabled}
                onClick={onClick}
                onMouseUp={onMouseUp}
                onMouseDown={onMouseDown}
            >
                {
                    isLoading ?
                        <CircularProgress/>                    
                    :
                        children
                }
            </Button>
        </>
);

export default DataButton;