import React, { useEffect, useState } from "react";
import classes from "./DatasetDataSidebar.module.css";
import Item from "./components/Item/Item";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import DataButton from "../../utils/DataButton/DataButton";

const DataSidebar = ({ datasetId, title, dataUrl, items, setItems, itemUrl }) => {    
    const safeFetch = useSafeDataFetch()[1];
    const [hasMore, setHasMore] = useState(true);
    const [pageNumber, setPageNumber] = useState(1);
    
    useEffect(() => {
        const fetchItems = async () => {

            const response = await safeFetch({
                url: `${dataUrl}${datasetId}?pageNumber=${pageNumber}` 
            });

            if(!response.isError){
                setHasMore(response.data.length > 0);
                if(pageNumber === 1){
                    setItems(response.data);
                } else {
                    setItems(pageNumber => [...pageNumber, ...response.data]);
                }
            }
        };

        if(datasetId){
            fetchItems();
        }

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pageNumber, datasetId, dataUrl]);

    return (
        <div className={classes.dataSidebar}>
            <h6>{title}</h6>
            <hr/>
            <div className={classes.items}>
                {
                    items.map(item => (
                        <Item
                            itemUrl={itemUrl}
                            key={item.id}
                            data={item}
                        />
                    ))
                }
                <div className={classes.buttonContainer}>
                    <DataButton
                        disabled={!hasMore}
                        onClick={() => setPageNumber(pageNumber => pageNumber + 1)}
                    >
                        Show More
                    </DataButton>
                </div>
            </div>
        </div>
    )
};

export default DataSidebar;