import React, { useEffect } from "react";
import { useState } from "react";
import classes from "./ChatLabeller.module.css";
import DatasetTitle from "../../components/DatasetTitle/DatasetTitle";
import DatasetModal from "../../components/DatasetModal/DatasetModal";
import Messages from "./components/Messages/Messages";
import ActionBar from "./components/ActionBar/ActionBar";
import { useParams } from "react-router-dom";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import DatasetDataSidebar from "../../components/DatasetDataSidebar/DatasetDataSidebar";

const ChatLabeller = () => {
    const [chat, setChat] = useState({});
    const safeDataFetch = useSafeDataFetch()[1];

    const [selectedDataset, setSelectedDataset] = useState({
        id: null,
        name: ""
    });

    const { chatId } = useParams();

    const [open, setOpen] = useState(chatId === undefined);
    const [pastChats, setPastChats] = useState([]);

    const datasetId = selectedDataset.id;

    useEffect(() => {
        const fetchChat = async () => {
            const response = await safeDataFetch({
                url: "/chat/get-by-id/" + chatId
            });

            if(!response.isError){
                setSelectedDataset(response.data.dataset);
                setChat(response.data);
            }
        };

        if(chatId){
            fetchChat();
        } else {
            setChat({
                name: "",
                messages: [
                    {
                        text: "",
                        isSystem: false
                    }
                ]
            })
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [chatId])

    return (
        <div className={classes.chatLabeller}>
            <DatasetModal
                open={open}
                setOpen={setOpen}
                selectedDataset={selectedDataset}
                setSelectedDataset={setSelectedDataset}
                handleClose={() => setOpen(false)}
            />
            <h5>Chat Labeller</h5>
            <DatasetTitle
                selectedDataset={selectedDataset}
                setOpen={setOpen}
            />
            <ActionBar/>
            <div className={classes.body}>
                <DatasetDataSidebar
                    datasetId={datasetId}
                    dataUrl="/chat/get-by-dataset/"
                    items={pastChats}
                    itemUrl="/chat-labeller/"
                    setItems={setPastChats}
                    title="Previous"
                />
                <Messages
                    chat={chat}
                    setChat={setChat}
                    datasetId={datasetId}
                    setPastChats={setPastChats}
                />
            </div>
        </div>
    )
};

export default ChatLabeller;